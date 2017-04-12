using MoreDapper.Converter.Types.Interfaces;

namespace MoreDapper.Converter.Types
{
    internal class DoubleConverter : SimpleNullableConverter<double>, IConverter
    {
        public string GetTypeName(object value)
        {
            return "float";
        }
    }
}
