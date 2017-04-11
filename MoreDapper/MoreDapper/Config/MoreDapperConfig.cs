using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreDapper.Config
{
    public static class MoreDapperConfig
    {
        private static List<string> GlobalPrimaryKeys = new List<string>
        {
            "Id"
        };

        private static Dictionary<Type, List<string>> PrimaryKeys = new Dictionary<Type, List<string>>();

        public static void RemovePrimaryKey(string property)
        {
            if (string.IsNullOrWhiteSpace(property))
            {
                throw new ArgumentNullException("property can not be null or white space.");
            }

            GlobalPrimaryKeys.Remove(property);
        }

        public static void AddPrimaryKey(string property)
        {
            if(property == null)
            {
                throw new ArgumentNullException("property can not be null.");
            }

            GlobalPrimaryKeys.Add(property);
        }

        public static void AddPrimaryKey(Type type, List<string> properties)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type can not be null.");
            }

            if (properties == null)
            {
                throw new ArgumentNullException("property can not be null.");
            }

            if (properties.Count == 0)
            {
                throw new ArgumentNullException("properties can not be empty.");
            }

            foreach (var property in properties)
            {
                if (string.IsNullOrWhiteSpace(property))
                {
                    throw new ArgumentNullException("property can not be null or white space.");
                }
            }

            PrimaryKeys.Add(type, properties);
        }

        public static List<string> GetKeysFor(Type type)
        {
            List<string> properties = new List<string>();

            if(!PrimaryKeys.TryGetValue(type, out properties))
            {
                properties = new List<string>();
            }
            properties.AddRange(GlobalPrimaryKeys);

            return properties;
        }
    }
}
