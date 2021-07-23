using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;
using Z2.Core.Web;
using Z2.Core.WebApp.Repository;
using Z2.Model.Token;

namespace Z2.WebAPI.Controllers
{
    public class AccountController : _ApiBaseController
    {
        [HttpPost]
        public ApiResponseMessage Login([FromBody]loginInfo info)
        {
            if (info == null || string.IsNullOrEmpty(info.userName) || string.IsNullOrEmpty(info.password))
            {
                return JsonHandler.CreateApiResponseMessage(1, Resources.BaseRes.Account_InputUserNameAndPassword);
            }

            //[FromBody] loginInfo info
            //[FromBody]string userName, [FromBody] string password
            string userName = info.userName;
            string password = info.password;

            //string userName = "baibao";
            //string password = "123456";

            var model = new UserRep().CheckLogin(userName, password);
            if (model == null)
            {
                return JsonHandler.CreateApiResponseMessage(1, "用户名或密码错误");
            }

            FormsAuthenticationTicket token = new FormsAuthenticationTicket(0, userName, DateTime.Now,
                            DateTime.Now.AddHours(8), true, string.Format("{0}&{1}", userName, password),
                            FormsAuthentication.FormsCookiePath);
            //返回登录结果、用户信息、用户验证票据信息
            var Token = FormsAuthentication.Encrypt(token);
            //将身份信息保存在session中，验证当前请求是否是有效请求
            //HttpContext.Current.Session[userName] = Token;

            var tokenModel = new UserTokenModel()
            {
                UserId = model.Id,
                RoleId = model.RoleId,
                Timeout = token.Expiration,
                Token = Token
            };

            UserTokenManager.Instance.AddToken(tokenModel);
            var outModel = new Oup_UserInfo()
            {
                Token = Token,
                UserId = model.Id,
                UserName = model.Account,
                TrueName = model.NickName,
                DepId = model.DepartmentId,
                DepName = model.DepartmentName,
            };    

            return JsonHandler.CreateSuccessApiMessage(outModel);
        }
        [System.Web.Http.HttpPost]
        public ApiResponseMessage SignOut()
        {
            string token = string.Empty;
            var ts = this.Request.Headers.Where(c => c.Key.ToLower() == UserTokenManager.TOKENNAME).FirstOrDefault().Value;
            if (ts != null && ts.Count() > 0)
            {
                token = ts.First<string>();
            }
            if (string.IsNullOrEmpty(token) && this.Request.Properties.ContainsKey(UserTokenManager.TOKENNAME))
            {
                token = $"{this.Request.Properties[UserTokenManager.TOKENNAME]}";
            }

            if (string.IsNullOrEmpty(token))
            {
                return JsonHandler.CreateApiResponseMessage(2, ErrorCode.ErrCodes[2]);
            }
            //var tokens = this.Request.Headers.GetValues("token");
            //var utm = new UserTokenManager();

            var tokenModel = new UserTokenModel()
            {
                Token = token
            };
            UserTokenManager.Instance.RemoveToken(tokenModel);

            return JsonHandler.CreateSuccessApiMessage(new ApiMessageValue() { Message = "已退出" });
        }

    }
}
