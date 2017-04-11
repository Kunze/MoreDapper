using MoreDapper.Converter.Types;
using MoreDapper.Converter.Types.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MoreDapper
{
    public class SqlTypeConverter
    {
        public SqlTypeConverter()
        {
            Converters = new List<IConverter>
            {
                new StringConverter(),
                new FloatConverter(),
                new DecimalConverter(),
                new BooleanConverter(),
                new IntConverter(),
                new DateTimeConverter(),
                new CharConverter()
            };
        }

        private IList<IConverter> Converters { get; set; }

        public void AddConverter(IConverter converter)
        {
            if(converter == null)
            {
                throw new ArgumentNullException("converter can not be null.");
            }

            Converters.Add(converter);
        }

        internal string GetValue(object obj, PropertyInfo property)
        {
            foreach (var converter in Converters)
            {
                if (converter.Match(property))
                {
                    return converter.GetValue(obj, property);
                }
            }

            throw new Exception($"Invalid type {property.GetType().Name}");
        }

        internal string GetPropertyType<T>(T obj, PropertyInfo property)
        {
            foreach (var converter in Converters)
            {
                if (converter.Match(property))
                {
                    return converter.GetTypeName(property.GetValue(obj));
                }
            }

            throw new Exception($"Invalid type {property.GetType().Name}");
        }

        internal static Lazy<SqlTypeConverter> Instance = new Lazy<SqlTypeConverter>(() =>
        {
            return new SqlTypeConverter();
        });

        internal static SqlTypeConverter GetInstance()
        {
            return Instance.Value;
        }
    }
}
