using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.WebApp.Model
{

    /// <summary>
    /// 角色授权表
    /// </summary>
    [Serializable]
    public partial class SysRoleAuthorize
    {
        public SysRoleAuthorize()
        { }
        #region Model
        private string _id;
        private int? _itemtype;
        private string _itemid;
        private int? _objecttype;
        private string _objectid;
        private int? _displayno;
        private DateTime? _creatertime;
        private string _createruserid;
        /// <summary>
        /// 角色授权主键
        /// </summary>
        public string Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 项目类型1-模块2-按钮3-列表
        /// </summary>
        public int? ItemType
        {
            set { _itemtype = value; }
            get { return _itemtype; }
        }
        /// <summary>
        /// 项目主键
        /// </summary>
        public string ItemId
        {
            set { _itemid = value; }
            get { return _itemid; }
        }
        /// <summary>
        /// 对象分类1-角色2-部门-3用户
        /// </summary>
        public int? ObjectType
        {
            set { _objecttype = value; }
            get { return _objecttype; }
        }
        /// <summary>
        /// 对象主键
        /// </summary>
        public string ObjectId
        {
            set { _objectid = value; }
            get { return _objectid; }
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
        /// 创建时间
        /// </summary>
        public DateTime? CreaterTime
        {
            set { _creatertime = value; }
            get { return _creatertime; }
        }
        /// <summary>
        /// 创建用户
        /// </summary>
        public string CreaterUserId
        {
            set { _createruserid = value; }
            get { return _createruserid; }
        }
        #endregion Model

    }
}
