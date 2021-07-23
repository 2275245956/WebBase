using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z2.Core.Interface;
using Z2.Core.Web;

namespace Z2.Web.Areas.DicMng.Controllers
{

    [ModuleAction(ModuleId = "E0DA0F96-2560-44F6-86FD-B09D0D8441D4", ModuleName = "证书类型", ParentModuleId = "8A824B5F6-2974-41DD-8887-4CA1C9C68367", DisplayNo = 5)]
    public class CertificateTypeController : DicBaseController
    {
        // GET: DicMng/CertificateType
        public override string CategoryCode => "Certificate";
    }
}