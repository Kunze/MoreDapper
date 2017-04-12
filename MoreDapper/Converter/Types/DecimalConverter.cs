using MoreDapper.Converter.Types.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreDapper.Converter.Types
{
    internal class DecimalConverter : SimpleNullableConverter<decimal>, IConverter
    {
        public string GetTypeName(object value)
        {
            return "float";
        }
    }
}
