using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreDapper.Scanner
{
    internal class Parameter
    {
        internal int StartIndex { get; set; }
        internal string Name { get; set; }

        internal Parameter(int startindex, int endIndex, string name)
        {
            if (startindex < 0)
            {
                throw new ArgumentException("startindex can not be less than 0.");
            }

            if (endIndex < 0)
            {
                throw new ArgumentException("endIndex can not be less than 0.");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name can not be null or white space.");
            }

            StartIndex = startindex;
            Name = name;
        }

        internal string GetParameterName()
        {
            return Name.Remove(0, 1);
        }
    }
}
