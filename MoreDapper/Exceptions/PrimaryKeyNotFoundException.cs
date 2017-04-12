using System;

namespace MoreDapper.Exceptions
{
    public class PrimaryKeyNotFoundException : Exception
    {
        public PrimaryKeyNotFoundException(string message) : base(message)
        {
        }
    }
}
