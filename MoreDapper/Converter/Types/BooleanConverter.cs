using MoreDapper.Converter.Types.Interfaces;
using System;
using System.Reflection;

namespace MoreDapper.Converter.Types
{
    internal class BooleanConverter : IConverter
    {
        public string GetTypeName(object value)
        {
            return "bit";
        }

        public string GetValue(object obj, PropertyInfo property)
        {
            var value = property.GetValue(obj);
            if (value == null)
            {
                return "null";
            }

            return Convert.ToBoolean(value) ? "1" : "0";
        }

        public bool Match(PropertyInfo property)
        {
            Type t = property.PropertyType;
            t = Nullable.GetUnderlyingType(t) ?? t;

            return t == typeof(bool);
        }
    }
}
