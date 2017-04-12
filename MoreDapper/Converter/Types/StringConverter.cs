using MoreDapper.Converter.Types.Interfaces;
using System.Reflection;

namespace MoreDapper.Converter.Types
{
    internal class StringConverter : IConverter
    {
        public string GetTypeName(object value)
        {
            return $"varchar({value.ToString().Length})";
        }

        public string GetValue(object obj, PropertyInfo property)
        {
            var value = property.GetValue(obj) as string;
            if (value == null)
            {
                return "null";
            }

            value.Replace("'", "''");//<TODO>criar converter para isso?

            return $"'{value}'";
        }

        public bool Match(PropertyInfo property)
        {
            return property.PropertyType == typeof(string);
        }
    }
}
