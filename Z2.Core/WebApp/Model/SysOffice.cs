using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.WebApp.Model
{

    [TableName(TableName = "SysOffice", LinkTableName = "SysOfficeUser", LinkTableField = "OfficeId")]
    public class SysOffice : MasterModelBase<string>
    {
        public override string MasterId
        {
            get
            {
                return this.Id;
            }

            set
            {
                this.Id = value;
            }
        }

        public override string MasterName
        {
            get
            {
                return this.Name;
            }

            set
            {
                this.Name = value;
            }
        }

        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ParentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string EnCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SimpleSpelling { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Layers { get; set; }

        public string Description { get; set; }


       
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? WithdrawalDate { get; set; }

    }
}
