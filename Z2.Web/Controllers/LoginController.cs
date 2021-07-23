using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core;
using Z2.Core.Exceptions;
using Z2.Core.Handler;
using Z2.Core.Operator;
using Z2.Core.Security;
using Z2.Core.Utility;
using Z2.Core.Web;
using Z2.Core.WebApp.Model;
using Z2.Core.WebApp.Repository;
using Z2.Web.Filter;

namespace Z2.Web.Controllers
{
    [Localization]
    public class LoginController : AppBaseController
    {
        private Mall_BrandRep mbr = new Mall_BrandRep();
        private SysSettingRep settingRep = new SysSettingRep();

        public override string ModuleId
        {
            get { return "LoginController"; }
        }

        public override string ModuleName
        {
            get { return "系统登录"; }
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            return View();
        }
        //[HttpGet]
        //public ActionResult GetAuthCode()
        //{
        //    return File(new VerifyCode().GetVerifyCode(), @"image/Gif");
        //}

        [HttpPost]
        public ActionResult Logout()
        {
            this.WriteInfo(ResultType.success.ToString(), "注销", null);
            Session.Abandon();
            Session.Clear();
            OperatorProvider.Provider.RemoveCurrent();
            return Content(new ApiResponseMessage { state = ResultType.success.ToString(), message = "注销成功。" }.ToJson());
        }


        private int _expires = 60 * 8; //默认时间  8小时
        [HttpPost]
        [HttpAjaxOnly]
        public ActionResult CheckLogin(string username, string password, bool isRemPwd)
        {

            try
            {
                var userEntity = new UserRep().CheckLogin(username, password);
                if (userEntity != null)
                {
                    var operatorModel = new OperatorModel
                    {
                        UserId = userEntity.Id,
                        UserCode = userEntity.Account,
                        UserPwd = password,
                        UserName = userEntity.RealName,
                        CompanyId = userEntity.OrganizeId,
                        DepartmentId = userEntity.DepartmentId,
                        RoleId = userEntity.RoleId,
                        LoginIPAddress = Net.Ip,
                        LoginTime = DateTime.Now,
                        LoginToken = DESEncryptHelper.Encrypt(Guid.NewGuid().ToString()),
                        IsSystem = userEntity.Account == "admin"
                    };
                    //operatorModel.LoginIPAddressName = Net.GetLocation(operatorModel.LoginIPAddress);

                    this.WriteInfo(ResultType.success.ToString(), Resources.BaseRes.Login_LoginSuccess,
                        new { UserId = operatorModel.UserId, username = username });
                    ////如果选择了记住密码     一个月
                    if (isRemPwd)
                    {
                        _expires = 30 * 3 * _expires;

                    }

                    OperatorProvider.Provider.AddCurrent(operatorModel, _expires);
                    return Content(new ApiResponseMessage
                    { state = ResultType.success.ToString(), message = Resources.BaseRes.Login_LoginSuccess }
                        .ToJson());
                }
                else
                {
                    this.WriteInfo(ResultType.error.ToString(), Resources.BaseRes.ErrorInfo,
                        new { username = username, password = password });
                    return Content(new ApiResponseMessage
                    { state = ResultType.error.ToString(), message = Resources.BaseRes.ErrorInfo }.ToJson());
                }
            }
            catch (AppException ex)
            {
                this.WriteInfo(ResultType.error.ToString(), ex.Message, new { username = username, password = password });
                return Content(
                    new ApiResponseMessage { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
            catch (Exception ex)
            {
                this.WriteInfo(ResultType.error.ToString(), ex.Message, new { username = username, password = password });
                return Content(
                    new ApiResponseMessage { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }


        /// <summary>
        /// 记住密码 自动登陆
        /// </summary>
        /// <param name="info">Coolkie信息</param>
        /// <param name="pwd">pwd</param>
        /// <returns></returns>
        [HttpPost]
        [HttpAjaxOnly]
        public ActionResult AutoLoginCheck()
        {
            //解密 Cookie信息

            var operatorModel = DESEncryptHelper
                .Decrypt(WebHelper.GetCookie("z2_loginuserkey_2016").ToString())
                .ToObject<OperatorModel>();
            try
            {
                if (operatorModel != null)
                {
                    if (string.IsNullOrEmpty(operatorModel.UserPwd))
                    {
                        return null;

                    }

                    var userEntity = new UserRep().CheckLogin(operatorModel.UserCode, operatorModel.UserPwd);
                    if (userEntity != null)
                    {
                        operatorModel.UserId = userEntity.Id;
                        operatorModel.UserCode = userEntity.Account;
                        operatorModel.UserName = userEntity.RealName;
                        operatorModel.CompanyId = userEntity.OrganizeId;
                        operatorModel.DepartmentId = userEntity.DepartmentId;
                        operatorModel.RoleId = userEntity.RoleId;
                        operatorModel.LoginIPAddress = Net.Ip;
                        operatorModel.LoginTime = DateTime.Now;
                        operatorModel.LoginToken = DESEncryptHelper.Encrypt(Guid.NewGuid().ToString());
                        operatorModel.IsSystem = userEntity.Account == "admin";

                        this.WriteInfo(ResultType.success.ToString(), Resources.BaseRes.Login_LoginSuccess,
                            new { UserId = operatorModel.UserId, username = operatorModel.UserName });

                        OperatorProvider.Provider.AddCurrent(operatorModel, _expires);
                        return Content(new ApiResponseMessage
                        { state = ResultType.success.ToString(), message = Resources.BaseRes.Login_LoginSuccess }
                            .ToJson());
                    }
                    else
                    {
                        this.WriteInfo(ResultType.error.ToString(), Resources.BaseRes.ErrorInfo,
                            new { username = operatorModel.UserCode, password = operatorModel.UserPwd });
                        return Content(new ApiResponseMessage
                        { state = ResultType.error.ToString(), message = Resources.BaseRes.ErrorInfo }.ToJson());
                    }

                }
                else
                {
                    return Content("");
                }
            }
            catch (AppException ex)
            {
                if (operatorModel != null)
                    this.WriteInfo(ResultType.error.ToString(), ex.Message,
                        new { username = operatorModel.UserCode, password = operatorModel.UserPwd });
                return Content(
                    new ApiResponseMessage { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
            catch (Exception ex)
            {
                if (operatorModel != null)
                    this.WriteInfo(ResultType.error.ToString(), ex.Message,
                        new { username = operatorModel.UserCode, password = operatorModel.UserPwd });
                return Content(
                    new ApiResponseMessage { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }


        /// <summary>
        /// 获取数据库图片路径通过FilePathResult返回地址传送给客户端
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginImage()
        {
            var data = settingRep.GetSetting("SysLogo", "");
            //var fr = new FilePathResult(data, ContentTypeHelper.ContentType(".jpg"));//获取查询到文件的路径通过FilePathResult以路径形式传送到客户端
            return Content(data);
        }

        [HttpPost]
        public ActionResult GetHomeColor()
        {
            var data = mbr.GetHomePage();
            var HomeColor = data[0].Description;
            return Json(HomeColor);
        }
    }
}
