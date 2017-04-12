using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MoreDapper.Converter.Types.Interfaces
{
    public interface IConverter
    {
        string GetValue(object obj, PropertyInfo property);
        string GetTypeName(object value);
        //Tuple<string, string> GetInfo(object obj, PropertyInfo property);
        bool Match(PropertyInfo property);
    }
}
