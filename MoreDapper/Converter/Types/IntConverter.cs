using System.Reflection;
using MoreDapper.Converter.Types.Interfaces;
using System;
using System.Globalization;

namespace MoreDapper.Converter.Types
{
    internal class IntConverter : IConverter
    {
        public string GetTypeName(object value)
        {
            return "int";
        }

        public string GetValue(object obj, PropertyInfo property)
        {
            var value = property.GetValue(obj);

            if (value == null)
            {
                return "null";
            }

            return Convert.ToString(value, CultureInfo.InvariantCulture);
        }

        public bool Match(PropertyInfo property)
        {
            Type t = property.PropertyType;
            t = Nullable.GetUnderlyingType(t) ?? t;

            return t == typeof(Int16) 
                || t == typeof(Int32) 
                || t == typeof(Int64) 
                || t == typeof(UInt16) 
                || t == typeof(UInt32) 
                || t == typeof(UInt64);
        }
    }
}
