using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.WebApp.Model
{
    public class SelectListItem
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }


    public class TableNameAttribute : Attribute
    {
        private string _tableId;
        private string _tableTitle;

        public string TableId
        {
            get
            {
                if (string.IsNullOrEmpty(_tableId))
                {
                    return this.TableName;
                }
                else { return _tableId; }
            }
            set
            {
                _tableId = value;
            }
        }

        public string TableName { get; set; }

        public string TableTitle
        {
            get
            {
                if (string.IsNullOrEmpty(_tableTitle))
                {
                    return this.TableName;
                }
                else { return _tableTitle; }
            }
            set
            {
                _tableTitle = value;
            }
        }

        public string KeyField { get; set; }

        public string NameField { get; set; }

        public string CreaterUserIdFieldName { get; set; }
        public string CreaterTimeFieldName { get; set; }

        public string LastUpdateTimeFieldName { get; set; }

        public string LastUpdateUserIdFieldName { get; set; }

        public string DeleteTimeFieldName { get; set; }

        public string DeleteUserIdFieldName { get; set; }

        public bool AutoID { get; set; }

        public string LinkTableName { get; set; }
        public string LinkTableField { get; set; }

        public TableNameAttribute()
        {
            KeyField = "Id";
            NameField = "Name";
            AutoID = false;
            CreaterUserIdFieldName = "CreaterUserId";
            CreaterTimeFieldName = "CreaterTime";

            LastUpdateTimeFieldName = "LastUpdateTime";
            LastUpdateUserIdFieldName = "LastUpdateUserId";

            DeleteTimeFieldName = "DeleteTim";
            DeleteUserIdFieldName = "DeleteUserId";
        }

    }

    public interface ISort
    {
        /// <summary>
        /// 
        /// </summary>
        int DisplayNo { get; set; }
    }

    public interface IMasterModel<T>: ISort
    {
        /// <summary>
        /// 
        /// </summary>
        T MasterId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string MasterName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        bool DeleteFlag { get; set; }

        /// <summary>
        /// 
        /// </summary>
        bool EnabledFlag { get; set; }

        /// <summary>
        /// 
        /// </summary>
        DateTime? CreaterTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        string CreaterUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        DateTime? LastUpdateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        string LastUpdateUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        DateTime? DeleteTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        string DeleteUserId { get; set; }
    }


    [TableName(TableName = "", KeyField = "", NameField = "")]
    public abstract class MasterModelBase<T> : IMasterModel<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public abstract T MasterId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "名称")]
        [Required()]
        public abstract string MasterName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual int DisplayNo { get; set; }
        /// <summary>
        /// 表示フラグ 0:非表示, 1:表示
        /// </summary>
        public virtual bool EnabledFlag { get; set; }

        public virtual bool DeleteFlag { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime? CreaterTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string CreaterUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime? LastUpdateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string LastUpdateUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime? DeleteTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string DeleteUserId { get; set; }
    }

}
