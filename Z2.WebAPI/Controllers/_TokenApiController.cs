using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Security;
using Z2.Core.Web;

namespace Z2.WebAPI.Controllers
{
    public class _TokenApiController : _ApiBaseController
    {
        #region CheckUserAndPermission
        protected string GetRequest(string key)
        {
            var content = Request.Properties[UserTokenManager.MS_HttpContext] as HttpContextBase;
            return content.Request.Form[key];
        }


        private string _token = null;
        private string _userId = null;

        protected bool CheckUserAndPermission(out ApiResponseMessage msg)
        {
            if (string.IsNullOrEmpty(_token) || string.IsNullOrEmpty(_userId))
            {
                msg = JsonHandler.CreateApiResponseMessage(2, ErrorCode.ErrCodes[2]);
                return false;
            }
            msg = null;
            return true;
        }

        /// <summary>
        /// 获取用户Id
        /// </summary>
        /// <returns></returns>
        protected virtual string GetUserId()
        {
            return _userId;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="controllerContext"></param>
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);

            //url获取token
            var content = controllerContext.Request.Properties[UserTokenManager.MS_HttpContext] as HttpContextBase;

            string token = string.Empty;
            var ts = controllerContext.Request.Headers.Where(c => c.Key.ToLower() == UserTokenManager.TOKENNAME).FirstOrDefault().Value;
            if (ts != null && ts.Count() > 0)
            {
                token = ts.First<string>();
            }
            if (string.IsNullOrEmpty(token))
            {
                token = content.Request.QueryString[UserTokenManager.TOKENNAME];
            }
            if (string.IsNullOrEmpty(token))
            {
                token = content.Request.Form[UserTokenManager.TOKENNAME];
            }
            //解密用户ticket,并校验用户名密码是否匹配
            if (!string.IsNullOrEmpty(token) && ValidateTicket(token))
            {
                _token = token;
                //var utm = new UserTokenManager();
                _userId = UserTokenManager.Instance.GetUId(token);
                //if (!string.IsNullOrEmpty(m_Token))
                //{
                //    return DecryptTokenToUserName(m_Token);
                //}
                //else
                //{
                //    return null;
                //}
            }
        }

        bool ValidateTicket(string encryptToken)
        {
            //var utm = new UserTokenManager();
            if (!UserTokenManager.Instance.IsExistToken(encryptToken))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private string DecryptTokenToUserName(string encryptToken)
        {
            var strTicket = FormsAuthentication.Decrypt(encryptToken.Trim());
            var userData = strTicket.UserData;
            //从Ticket里面获取用户名和密码
            var index = userData.IndexOf("&");
            string userName = userData.Substring(0, index);
            //string password = strTicket.Substring(index + 1);
            return userName;
        }

        #endregion

    }
}
