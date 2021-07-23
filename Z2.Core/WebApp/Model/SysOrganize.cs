using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.WebApp.Model
{

    /// <summary>
    /// 组织表
    /// </summary>
    [Serializable]
    public partial class SysOrganize
    {
        public SysOrganize()
        { }
        #region Model
        private string _id;
        private string _parentid;
        private int? _layers;
        private string _encode;
        private string _name;
        private string _shortname;
        private string _categoryid;
        private string _managerid;
        private string _telephone;
        private string _mobilephone;
        private string _wechat;
        private string _fax;
        private string _email;
        private string _areaid;
        private string _address;
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
        /// 组织主键
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
        /// 简称
        /// </summary>
        public string ShortName
        {
            set { _shortname = value; }
            get { return _shortname; }
        }
        /// <summary>
        /// 分类
        /// </summary>
        public string CategoryId
        {
            set { _categoryid = value; }
            get { return _categoryid; }
        }
        /// <summary>
        /// 负责人
        /// </summary>
        public string ManagerId
        {
            set { _managerid = value; }
            get { return _managerid; }
        }
        /// <summary>
        /// 电话
        /// </summary>
        public string TelePhone
        {
            set { _telephone = value; }
            get { return _telephone; }
        }
        /// <summary>
        /// 手机
        /// </summary>
        public string MobilePhone
        {
            set { _mobilephone = value; }
            get { return _mobilephone; }
        }
        /// <summary>
        /// 微信
        /// </summary>
        public string WeChat
        {
            set { _wechat = value; }
            get { return _wechat; }
        }
        /// <summary>
        /// 传真
        /// </summary>
        public string Fax
        {
            set { _fax = value; }
            get { return _fax; }
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// 归属区域
        /// </summary>
        public string AreaId
        {
            set { _areaid = value; }
            get { return _areaid; }
        }
        /// <summary>
        /// 联系地址
        /// </summary>
        public string Address
        {
            set { _address = value; }
            get { return _address; }
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
        #endregion Model

    }
}
