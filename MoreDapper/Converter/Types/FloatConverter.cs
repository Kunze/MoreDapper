using MoreDapper.Converter.Types.Interfaces;

namespace MoreDapper.Converter.Types
{
    internal class FloatConverter : SimpleNullableConverter<float>, IConverter
    {
        public string GetTypeName(object value)
        {
            return "float";
        }
    }
}
