using MoreDapper.Converter.Types.Interfaces;
using System;

namespace MoreDapper.Converter.Types
{
    internal class DateTimeConverter : SimpleNullableConverter<DateTime>, IConverter
    {
        public string GetTypeName(object value)
        {
            return "datetime";
        }
    }
}
