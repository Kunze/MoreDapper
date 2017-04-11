using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreDapper.Exceptions
{
    public class PrimaryKeyNotFoundException : Exception
    {
        public PrimaryKeyNotFoundException(string message) : base(message)
        {
        }
    }
}
