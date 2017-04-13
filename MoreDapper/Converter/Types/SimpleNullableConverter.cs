using System;
using System.Globalization;
using System.Reflection;

namespace MoreDapper.Converter.Types
{
    internal abstract class SimpleNullableConverter<T>
    {
        public virtual string GetValue(object obj, PropertyInfo property)
        {
            var value = property.GetValue(obj);

            if(value == null)
            {
                return "null";
            }

            return Convert.ToString(value, CultureInfo.InvariantCulture);
        }

        public virtual bool Match(PropertyInfo property)
        {
            Type t = property.PropertyType;
            t = Nullable.GetUnderlyingType(t) ?? t;

            return t == typeof(T);
        }
    }
}
