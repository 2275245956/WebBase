using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.WebApp.Model
{

    /// <summary>
    /// 用户表
    /// </summary>
    [Serializable]
    public partial class SysUser
    {
        public SysUser()
        { }
        #region Model
        private string _id;
        private string _account;
        private string _realname;
        private string _nickname;
        private string _avatar;
        private bool _gender;
        private DateTime? _birthday;
        private string _mobilephone;
        private string _email;
        private string _wechat;
        private string _managerid;
        private int? _securitylevel;
        private string _signature;
        private string _organizeid;
        private string _organizename;
        private string _departmentid;
        private string _departmentname;
        private string _roleid;
        private string _dutyid;
        private string _dutyname;
        private bool _isadministrator;
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
        private string _password;
        /// <summary>
        /// 用户主键
        /// </summary>
        public string Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 账户
        /// </summary>
        public string Account
        {
            set { _account = value; }
            get { return _account; }
        }
        /// <summary>
        /// 姓名
        /// </summary>
        public string RealName
        {
            set { _realname = value; }
            get { return _realname; }
        }
        /// <summary>
        /// 呢称
        /// </summary>
        public string NickName
        {
            set { _nickname = value; }
            get { return _nickname; }
        }
        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar
        {
            set { _avatar = value; }
            get { return _avatar; }
        }
        /// <summary>
        /// 性别
        /// </summary>
        public bool Gender
        {
            set { _gender = value; }
            get { return _gender; }
        }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday
        {
            set { _birthday = value; }
            get { return _birthday; }
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
        /// 邮箱
        /// </summary>
        public string Email
        {
            set { _email = value; }
            get { return _email; }
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
        /// 主管主键
        /// </summary>
        public string ManagerId
        {
            set { _managerid = value; }
            get { return _managerid; }
        }
        /// <summary>
        /// 安全级别
        /// </summary>
        public int? SecurityLevel
        {
            set { _securitylevel = value; }
            get { return _securitylevel; }
        }
        /// <summary>
        /// 个性签名
        /// </summary>
        public string Signature
        {
            set { _signature = value; }
            get { return _signature; }
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
        /// 组织名字
        /// </summary>
        public string OrganizeName
        {
            set { _organizename = value; }
            get { return _organizename; }
        }
        /// <summary>
        /// 部门主键
        /// </summary>
        public string DepartmentId
        {
            set { _departmentid = value; }
            get { return _departmentid; }
        }
        /// <summary>
        /// 部门名字
        /// </summary>
        public string DepartmentName
        {
            set { _departmentname = value; }
            get { return _departmentname; }
        }
        /// <summary>
        /// 角色主键
        /// </summary>
        public string RoleId
        {
            set { _roleid = value; }
            get { return _roleid; }
        }
        /// <summary>
        /// 岗位主键
        /// </summary>
        public string DutyId
        {
            set { _dutyid = value; }
            get { return _dutyid; }
        }

        public string DutyName
        {
            set { _dutyname = value; }
            get { return _dutyname; }
        }
        /// <summary>
        /// 是否管理员
        /// </summary>
        public bool IsAdministrator
        {
            set { _isadministrator = value; }
            get { return _isadministrator; }
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
        public string Password
        {
            set { _password = value; }
            get { return _password; }
        }
        #endregion Model
        public decimal JiaDongLv { get; set; }
    }
}
