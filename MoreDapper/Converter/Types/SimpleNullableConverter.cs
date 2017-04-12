using MoreDapper.Converter.Types.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MoreDapper.Converter.Types
{
    internal abstract class SimpleNullableConverter<T>
    {
        public string GetValue(object obj, PropertyInfo property)
        {
            var value = property.GetValue(obj);

            if(value == null)
            {
                return "null";
            }

            return Convert.ToString(value, CultureInfo.InvariantCulture);
        }

        public bool Match(PropertyInfo property)
        {
            Type t = property.PropertyType;
            t = Nullable.GetUnderlyingType(t) ?? t;

            return t == typeof(T);
        }
    }
}
