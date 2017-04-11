using MoreDapper.Converter.Types.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
