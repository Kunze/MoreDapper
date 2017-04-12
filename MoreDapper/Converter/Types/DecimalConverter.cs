using MoreDapper.Converter.Types.Interfaces;

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
