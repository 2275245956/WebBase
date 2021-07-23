using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.WebApp.Model
{
    public class RenWuMingXiModel
    {
        public string TaskId { get; set; }

        public string ProjectName { get; set; }

        public string LinkTaskId { get; set; }

        public string TaskName { get; set; }

        public string AssignTypeId { get; set; }

        public string AssignTypeName { get; set; }

        public string AssignDescription { get; set; }

        public DateTime Date { get; set; }

        public string DateStr { get {
                return string.Format("{0:yyyy/MM/dd}",Date).ToString();
            } }

        public decimal ActualHours { get; set; }

        public decimal AssignPercent { get; set; }

        public string RealName { get; set; }
    }
}
