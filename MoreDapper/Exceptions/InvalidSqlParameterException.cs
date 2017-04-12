using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreDapper.Exceptions
{
    public class InvalidSqlParameterException : Exception
    {
        public InvalidSqlParameterException(string message) : base(message)
        {
        }
    }
}
