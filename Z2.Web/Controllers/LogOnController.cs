using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Model.Token;
using System.Web.Security;
using Z2.Core;
using Z2.Core.Exceptions;
using Z2.Core.Handler;
using Z2.Core.Operator;
using Z2.Core.Security;
using Z2.Core.Web;
using Z2.Core.WebApp.Repository;
using Z2.Web.Token;

namespace Z2.Web.Controllers
{
    public class LogOnController : HandlerLoginInfoController
    {
        // GET: LogOn
      
        [HttpPost]
        [HttpAjaxOnly]
        public ActionResult CheckLogin(string username, string password)
        {
            try
            {
                //if (Session["z2_session_verifycode"].IsEmpty() || Md5.md5(code.ToLower(), 16) != Session["nfine_session_verifycode"].ToString())
                //{
                //    throw new Exception("验证码错误，请重新输入");
                //}

                var userEntity = new UserRep().CheckLogin(username, password);
                if (userEntity != null)
                {
                    var operatorModel = new OperatorModel();
                    operatorModel.UserId = userEntity.Id;
                    operatorModel.UserCode = userEntity.Account;
                    operatorModel.UserName = userEntity.RealName;
                    operatorModel.CompanyId = userEntity.OrganizeId;
                    operatorModel.DepartmentId = userEntity.DepartmentId;
                    operatorModel.RoleId = userEntity.RoleId;
                    operatorModel.LoginIPAddress = Net.Ip;
                    //operatorModel.LoginIPAddressName = Net.GetLocation(operatorModel.LoginIPAddress);
                    operatorModel.LoginTime = DateTime.Now;
                    operatorModel.LoginToken = DESEncryptHelper.Encrypt(Guid.NewGuid().ToString());
                    var tokenState = ConfigurationManager.AppSettings["TokenState"];
                    var version = ConfigurationManager.AppSettings["webpages:Version"];
                    FormsAuthenticationTicket token = new FormsAuthenticationTicket(Convert.ToInt32(version), userEntity.Id, DateTime.Now, DateTime.Now.AddMinutes(10), true, string.Format("{0}&{1}", userEntity.Id, password),
                        FormsAuthentication.FormsCookiePath);
                    var Token = FormsAuthentication.Encrypt(token);
                    var tokenModel = new UserTokenModel()
                    {
                        UserId = userEntity.Id,
                        Timeout = token.Expiration,
                        //RoleId = userEntity.RoleId,
                        Token = Token
                    };
                    var utm = new UserTokenManager();
                    utm.AddToken(tokenModel);

                    //AccountRep.InsertLoginLog(userEntity.Id, WebHelper.GetIP());

                    if (userEntity.Account == "admin")
                    {
                        operatorModel.IsSystem = true;
                    }
                    else
                    {
                        operatorModel.IsSystem = false;
                    }
                    OperatorProvider.Provider.AddCurrent(operatorModel);
                }
                return Content(new ApiResponseMessage { state = ResultType.success.ToString(), message = "登录成功。" }.ToJson());
            }
            catch (AppException ex)
            {
                return Content(new ApiResponseMessage { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new ApiResponseMessage { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }

    }
}