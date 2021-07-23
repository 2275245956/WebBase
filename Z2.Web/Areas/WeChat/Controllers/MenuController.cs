using System.Linq;
using System.Web.Mvc;
using Z2.Core.Handler;
using Z2.Core.Interface;
using Z2.Core.Web;
using Z2.Core.WebApp.Model;
using Z2.Core.WebApp.Repository;

namespace Z2.Web.Areas.WeChat.Controllers
{
    [ModuleAction(ModuleId = "3E6D7DBC-F33B-47DF-8248-4E30D7DE1DF7", ModuleName = "微信管理首页",ParentModuleId = "F415F732-A95A-43C7-B2DA-5BEA28C0DB6D")]
    public class MenuController : HandlerLoginInfoController
    {

        private readonly WxMpRep _wxMpRep = new WxMpRep();
        // GET:
        // WeChat/Menu

        [HttpGet]
        [HttpAjaxOnly]
        public ActionResult GetGridList(string keyword)
        {
            var data = _wxMpRep.GetList(keyword);
            var gridParam = new Pagination();
            TryUpdateModel(gridParam);
            var result = new
            {
                total = gridParam.GetPageTotal(data.Count()),
                page = gridParam.page,
                records = data.Count(),
                rows = data.Skip((gridParam.page - 1) * gridParam.rows).Take(gridParam.rows),

            };

            return Content(result.ToJson());
        }

        [HttpPost]
        [HttpAjaxOnly]
        [HttpAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string keyValue)
        {
            if (_wxMpRep.Delete(keyValue))
            {
                this.WriteInfo(ResultType.success.ToString(), "操作成功。", new { WId = keyValue });
                return Success("操作成功。");
            }
            else
            {
                this.WriteInfo(ResultType.error.ToString(), "操作失败", new { Wid = keyValue });
                return Error("操作失败！");
            }

        }

        /// <summary>
        /// 获取一条信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [HttpAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data=_wxMpRep.GetForm(keyValue);
            return Content(data.ToJson());
        }


        /// <summary>
        /// 增加   修改
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HttpAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(WxMp wxMp, string keyValue)
        {
            wxMp.Wid = keyValue;
            if (_wxMpRep.SubmitForm(wxMp))
            {
                this.WriteInfo(ResultType.success.ToString(), "操作成功。", new { WId = keyValue });
                return Success("操作成功。");
            }
            else
            {
                this.WriteInfo(ResultType.error.ToString(), "操作失败", new { Wid = keyValue });
                return Error("操作失败！");
            }


        }
    }
}