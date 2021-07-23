using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Web.Mvc;
using Z2.Core.WebApp.Model;
using System.Linq;

namespace Z2.Web.Extend
{
    public static class HtmlExtend
    {
        /// <summary>
        /// 刷新    
        /// </summary>
        /// <param name="helper">扩展类</param>
        /// <param name="id">Id</param>
        /// <returns></returns>
        public static MvcHtmlString ToolButtonRefresh(this HtmlHelper helper, string id)
        {
            return ToolButton(helper, id, "glyphicon glyphicon-refresh");
        }
        /// <summary>
        /// Create  增加      
        /// </summary>
        /// <param name="helper">扩展类</param>
        /// <param name="id">Id</param>
        /// <param name="codes">操作码</param>
        /// <returns></returns>
        public static MvcHtmlString ToolButtonAdd(this HtmlHelper helper, string id, List<SysRoleAuthorizeOperate> codes)
        {
            return ToolButton(helper, id, "fa fa-plus", "新建", codes, "Create");


        }
        /// <summary>
        /// Edit  编辑/修改    
        /// </summary>
        /// <param name="helper">扩展类</param>
        /// <param name="id">Id</param>
        /// <param name="codes">操作码</param>
        /// <returns></returns>
        public static MvcHtmlString ToolButtonEdit(this HtmlHelper helper, string id, List<SysRoleAuthorizeOperate> codes)
        {
            return ToolButton(helper, id, "fa fa-pencil-square-o", "修改", codes, "Edit");
        }
        /// <summary>
        /// Delete  删除    
        /// </summary>
        /// <param name="helper">扩展类</param>
        /// <param name="id">Id</param>
        /// <param name="codes">操作码</param>
        /// <returns></returns>
        public static MvcHtmlString ToolButtonDelete(this HtmlHelper helper, string id, List<SysRoleAuthorizeOperate> codes)
        {
            return ToolButton(helper, id, "fa fa-trash-o", "删除", codes, "Delete");

        }

        /// <summary>
        /// Detail  详细    
        /// </summary>
        /// <param name="helper">扩展类</param>
        /// <param name="id">Id</param>
        /// <param name="codes">操作码</param>
        /// <returns></returns>
        public static MvcHtmlString ToolButtonDetail(this HtmlHelper helper, string id, List<SysRoleAuthorizeOperate> codes)
        {
            return ToolButton(helper, id, "fa fa-share-square-o", "详细", codes, "Detail");
        }
        /// <summary>
        /// Export  导出    
        /// </summary>
        /// <param name="helper">扩展类</param>
        /// <param name="id">Id</param>
        /// <param name="codes">操作码</param>
        /// <returns></returns>
        public static MvcHtmlString ToolButtonExport(this HtmlHelper helper, string id, List<SysRoleAuthorizeOperate> codes)
        {
            return ToolButton(helper, id, "fa fa-file-excel-o", "导出", codes, "Export");
        }

        public static MvcHtmlString ToolButton(this HtmlHelper helper, string id, string icon)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("<div class='btn-group'>");
            sb.AppendFormat("<a id='{0}' class='btn btn-primary' ><span class='{1}'></span></a>", id, icon);
            sb.AppendFormat("</div>");
            return new MvcHtmlString(sb.ToString());
        }

        /// <summary>
        /// 普通按钮
        /// </summary>
        /// <param name="helper">htmlhelper</param>
        /// <param name="id">控件Id</param>
        /// <param name="icon">控件icon图标class</param>
        /// <param name="text">控件的名称</param>
        /// <param name="codes">操作码集合</param>
        /// <param name="code"></param>
        /// <returns>html</returns>
        public static MvcHtmlString ToolButton(this HtmlHelper helper, string id, string icon, string text, List<SysRoleAuthorizeOperate> codes, string code)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(code))
            {
                if (codes == null || codes.FindIndex(m => m.KeyCode == code) < 0)
                    return new MvcHtmlString(sb.ToString());
            }
            sb.AppendFormat("&nbsp;<div class='btn-group'>");
            sb.AppendFormat("<a id='{0}'authorize='true' class='btn btn-primary dropdown-text'><i class='{1}'></i>{2}</a>", id, icon, text);
            sb.AppendFormat("</div>");
            return new MvcHtmlString(sb.ToString());
        }


    }
}