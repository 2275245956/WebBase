using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2.Core.Exceptions
{
    public class AppException : Exception
    {
        public AppException() : base() { }
        public AppException(string message) : base(message) { }
        public AppException(string message, Exception innerException) : base(message, innerException) { }

    }

    public class IsUseingException : AppException
    {
        public IsUseingException() : base("他で使用されている為削除できません")
        {
        }
        public IsUseingException(string message) : base(message) { }
        public IsUseingException(string message, Exception innerException) : base(message, innerException) { }

    }
}
