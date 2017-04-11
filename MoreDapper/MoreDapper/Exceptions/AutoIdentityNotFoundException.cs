using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreDapper.Exceptions
{
    public class AutoIdentityNotFoundException : Exception
    {
        public AutoIdentityNotFoundException(string message) : base(message)
        {
        }
    }
}
