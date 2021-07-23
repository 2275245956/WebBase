using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.WebApp.Model
{

    /// <summary>
    /// 系统模块
    /// </summary>
    [Serializable]
    public partial class SysModule
    {
        public SysModule()
        { }
        #region Model
        private string _id;
        private string _parentid;
        private int? _layers;
        private string _encode;
        private string _name;
        private string _icon;
        private string _urladdress;
        private string _target;
        private bool _ismenu;
        private bool _isexpand;
        private bool _ispublic;
        private bool _allowedit;
        private bool _allowdelete;
        private int? _displayno;
        private bool _deleteflag;
        private bool _enabledflag;
        private string _description;
        private DateTime? _creatertime;
        private string _createruserid;
        private DateTime? _lastupdatetime;
        private string _lastupdateuserid;
        private DateTime? _deletetime;
        private string _deleteuserid;
        /// <summary>
        /// 模块主键
        /// </summary>
        public string Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 父级
        /// </summary>
        public string ParentId
        {
            set { _parentid = value; }
            get { return _parentid; }
        }
        /// <summary>
        /// 层次
        /// </summary>
        public int? Layers
        {
            set { _layers = value; }
            get { return _layers; }
        }
        /// <summary>
        /// 编码
        /// </summary>
        public string EnCode
        {
            set { _encode = value; }
            get { return _encode; }
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon
        {
            set { _icon = value; }
            get { return _icon; }
        }
        /// <summary>
        /// 连接
        /// </summary>
        public string UrlAddress
        {
            set { _urladdress = value; }
            get { return _urladdress; }
        }
        /// <summary>
        /// 目标
        /// </summary>
        public string Target
        {
            set { _target = value; }
            get { return _target; }
        }
        /// <summary>
        /// 菜单
        /// </summary>
        public bool IsMenu
        {
            set { _ismenu = value; }
            get { return _ismenu; }
        }
        /// <summary>
        /// 展开
        /// </summary>
        public bool IsExpand
        {
            set { _isexpand = value; }
            get { return _isexpand; }
        }
        /// <summary>
        /// 公共
        /// </summary>
        public bool IsPublic
        {
            set { _ispublic = value; }
            get { return _ispublic; }
        }
        /// <summary>
        /// 允许编辑
        /// </summary>
        public bool AllowEdit
        {
            set { _allowedit = value; }
            get { return _allowedit; }
        }
        /// <summary>
        /// 允许删除
        /// </summary>
        public bool AllowDelete
        {
            set { _allowdelete = value; }
            get { return _allowdelete; }
        }
        /// <summary>
        /// 排序码
        /// </summary>
        public int? DisplayNo
        {
            set { _displayno = value; }
            get { return _displayno; }
        }
        /// <summary>
        /// 删除标志
        /// </summary>
        public bool DeleteFlag
        {
            set { _deleteflag = value; }
            get { return _deleteflag; }
        }
        /// <summary>
        /// 有效标志
        /// </summary>
        public bool EnabledFlag
        {
            set { _enabledflag = value; }
            get { return _enabledflag; }
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreaterTime
        {
            set { _creatertime = value; }
            get { return _creatertime; }
        }
        /// <summary>
        /// 创建用户主键
        /// </summary>
        public string CreaterUserId
        {
            set { _createruserid = value; }
            get { return _createruserid; }
        }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? LastUpdateTime
        {
            set { _lastupdatetime = value; }
            get { return _lastupdatetime; }
        }
        /// <summary>
        /// 最后修改用户
        /// </summary>
        public string LastUpdateUserId
        {
            set { _lastupdateuserid = value; }
            get { return _lastupdateuserid; }
        }
        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeleteTime
        {
            set { _deletetime = value; }
            get { return _deletetime; }
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        public string DeleteUserId
        {
            set { _deleteuserid = value; }
            get { return _deleteuserid; }
        }

        /*
         *2018/11/12
         *范文强  添加属性  ModuleId
         */
        /// <summary>
        /// 模块Id
        /// </summary>
        public string ModuleId { get; set; }


        #endregion Model

    }
}
