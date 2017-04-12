using MoreDapper.Converter.Types.Interfaces;
using System;
using System.Reflection;

namespace MoreDapper.Converter.Types
{
    internal class CharConverter: IConverter
    {
        public string GetTypeName(object value)
        {
            return "char(1)";
        }

        public string GetValue(object obj, PropertyInfo property)
        {
            var value = property.GetValue(obj);
            if (value == null)
            {
                return "null";
            }

            var charValue = Convert.ToChar(value);
            if (charValue == '\'')
            {
                return "''''";
            }

            return $"'{charValue}'";
        }

        public bool Match(PropertyInfo property)
        {
            return property.PropertyType == typeof(char);
        }
    }
}
