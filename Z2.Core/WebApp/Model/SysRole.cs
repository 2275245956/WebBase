using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.WebApp.Model
{

    /// <summary>
    /// 角色表
    /// </summary>
    [Serializable]
    public partial class SysRole
    {
        public SysRole()
        { }
        #region Model
        private string _id;
        private string _organizeid;
        private string _organizename;
        private int? _category;
        private string _encode;
        private string _name;
        private string _type;
        private string _roleid;
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
        /// 角色主键
        /// </summary>
        public string Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 组织主键
        /// </summary>
        public string OrganizeId
        {
            set { _organizeid = value; }
            get { return _organizeid; }
        }
        /// <summary>
        /// 组织主键
        /// </summary>
        public string OrganizeName
        {
            set { _organizename = value; }
            get { return _organizename; }
        }
        /// <summary>
        /// 分类:1-角色2-岗位
        /// </summary>
        public int? Category
        {
            set { _category = value; }
            get { return _category; }
        }
        /// <summary>
        /// 编号
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
        /// 类型
        /// </summary>
        public string Type
        {
            set { _type = value; }
            get { return _type; }
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
        public string RoleId
        {
            set { _roleid = value; }
            get { return _roleid; }
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
