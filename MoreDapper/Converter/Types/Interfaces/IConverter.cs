using System.Reflection;

namespace MoreDapper.Converter.Types.Interfaces
{
    public interface IConverter
    {
        string GetValue(object obj, PropertyInfo property);
        string GetTypeName(object value);
        //Tuple<string, string> GetInfo(object obj, PropertyInfo property);
        bool Match(PropertyInfo property);
    }
}
