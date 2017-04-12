using MoreDapper.Converter.Types.Interfaces;

namespace MoreDapper.Converter.Types
{
    internal class IntConverter : SimpleNullableConverter<int>, IConverter
    {
        public string GetTypeName(object value)
        {
            return "int";
        }
    }
}
