using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.Utility
{
    public class ResultHelper
    {
        public static string NewId()
        {
            //string dt = DateTime.Now.ToString("yyyyMMddHHmmssfffffff");
            //dt (21) + guid (32)  
            var id = Guid.NewGuid().ToString("N");// string.Format("{0}{1}",dt, guid.ToString("N") );
            return id;
        }
    }
}
