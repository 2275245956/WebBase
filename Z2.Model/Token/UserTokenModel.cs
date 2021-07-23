using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Model.Token
{
    public class UserTokenModel
    {
        [Display(Name = "")] //
        public int ID { get; set; }
        [Display(Name = "")] //
        public string UserId { get; set; }
        [Display(Name = "")] //
        public string Token { get; set; }
        [Display(Name = "")] //
        public string LoginId { get; set; }
        [Display(Name = "")] //
        public string RoleId { get; set; }
        [Display(Name = "")] //
        public string DutyId { get; set; }
        [Display(Name = "")] //
        public DateTime Timeout { get; set; }
        [Display(Name = "")] //
        public string DepartmentId { get; set; }

    }
}
