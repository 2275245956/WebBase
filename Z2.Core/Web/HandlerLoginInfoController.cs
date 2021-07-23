using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DocumentFormat.OpenXml.EMMA;
using Z2.Core.DataBases;
using Z2.Core.Filter;
using Z2.Core.Handler;
using Z2.Core.Interface;
using Z2.Core.Logger;
using Z2.Core.WebApp.Model;
using Z2.Core.Operator;

namespace Z2.Core.Web
{


    [HttpLogin]
    /*
     *2018/10/31  
     *范文强  修改权限
     */
    //在Controller的基类中权限特性
    [LoginAuthority(AuthorityLevel = 2)]
    public abstract class HandlerLoginInfoController : AppBaseController
    {

        [HttpGet]
        [HttpAuthorize]
        public virtual ActionResult Index()
        {
            #region 获取操作码
            //var userId = "";
            //var currentUser = OperatorProvider.Provider.GetCurrent();
            //if (currentUser != null)
            //{
            //    userId = currentUser.UserId;
            //}
            //var codeList = GetOperateCodes(userId); //获取操作码
            //List<string> list = new List<string>();
            //if (codeList != null)
            //{
            //    foreach (var item in codeList)
            //    {
            //        list.Add(item.KeyCode);
            //    }
            //    ViewBag.codes = list;//添加到ViewBag  在前端访问
            //} 
            #endregion
            return View();
        }

        [HttpGet]
        [AuthoritiesCheck(OperationCode = new[] { "Create" })]
        public virtual ActionResult Create()
        {
            return View("Form");
        }

        [HttpGet]
        [AuthoritiesCheck(OperationCode = new[] { "Edit" })]
        public virtual ActionResult Edit()
        {
            return View("Form");
        }

        [HttpGet]
        [HttpAuthorize]
        public virtual ActionResult Form()
        {
            return View();
        }

        [HttpGet]
        [HttpAuthorize]
        public virtual ActionResult Details()
        {
            return View();
        }
        protected virtual ActionResult Success(string message)
        {
            return Content(new ApiResponseMessage { state = ResultType.success.ToString(), message = message }.ToJson());
        }
        protected virtual ActionResult Success(string message, object data)
        {
            return Content(new ApiResponseMessage { state = ResultType.success.ToString(), message = message, messageValue = data }.ToJson());
        }
        protected virtual ActionResult Error(string message)
        {
            return Content(new ApiResponseMessage { state = ResultType.error.ToString(), message = message }.ToJson());
        }


      

        /*
          2018/11/8  范文强 添加 获取用户针对某一模块所分配的操作码
         */
        /// <summary>
        /// 根据用户的角色Id和模块Id获取所有的操作码
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        protected List<SysRoleAuthorizeOperate> GetOperateCodes(string userId)
        {
            var moduleId = string.Empty;
            var module = this.GetType().GetCustomAttributes(typeof(ModuleActionAttribute), false);
            foreach (ModuleActionAttribute m in module)
            {
                moduleId = m.ModuleId;
            }
            string sql = @"SELECT    
                                           [Id]
                                          ,[AuthorizeId]
                                          ,[KeyCode]
                                          ,[IsValid]
                                          ,[Icon]
                                          ,[Name] 
                                    FROM SysRoleAuthorizeOperate
                                    WHERE IsValid = 0 and AuthorizeId in (
                                         SELECT Id FROM SysRoleAuthorize
                                         WHERE ItemId = @moduleId and ObjectId in (
                                                SELECT [RoleId] FROM[dbo].[SysRoleUser]
                                                WHERE UserId = @userId )
                                         )
                                    Order By Sort
                                          ";
            using (var db = DbUtility.GetInstance())
            {
                try
                {
                    var data = db.ReaderModelList<SysRoleAuthorizeOperate>(sql, new { moduleId = moduleId, userId = userId }).ToList();
                    return data;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        //获取操作码
        public List<SysRoleAuthorizeOperate> GetOperateCodes()
        {
            var currentUser = OperatorProvider.Provider.GetCurrent();//在session中去找当前登录人的userId
            if (currentUser != null)
            {
                return GetOperateCodes(currentUser.UserId);//通过userId去找所对应的操作码
            }

            return null;
        }

    }

}
