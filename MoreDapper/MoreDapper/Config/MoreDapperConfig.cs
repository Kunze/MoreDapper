using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreDapper.Config
{
    public static class MoreDapperConfig
    {
        private static List<string> GlobalAutoIdentity = new List<string>
        {
            "Id"
        };

        private static Dictionary<Type, List<string>> AutoIdentity = new Dictionary<Type, List<string>>();

        public static void RemoveAutoIdentity(string property)
        {
            if (string.IsNullOrWhiteSpace(property))
            {
                throw new ArgumentNullException("property can not be null or white space.");
            }

            GlobalAutoIdentity.Remove(property);
        }

        public static void AddAutoIdentity(string property)
        {
            if(property == null)
            {
                throw new ArgumentNullException("property can not be null.");
            }

            GlobalAutoIdentity.Add(property);
        }

        public static void AddAutoIdentity(Type type, List<string> properties)
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

            AutoIdentity.Add(type, properties);
        }

        public static bool Ignore(Type type, string property)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type can not be null.");
            }

            if (string.IsNullOrWhiteSpace(property))
            {
                throw new ArgumentNullException("property can not be null or white space.");
            }

            if (GlobalAutoIdentity.Contains(property))
            {
                return true;
            }

            List<string> properties;
            if(AutoIdentity.TryGetValue(type, out properties))
            {
                if (properties.Contains(property))
                {
                    return true;
                }
            }

            return false;
        }

        public static List<string> GetPropertiesFor(Type type)
        {
            List<string> properties = new List<string>();

            if(!AutoIdentity.TryGetValue(type, out properties))
            {
                properties = new List<string>();
            }
            properties.AddRange(GlobalAutoIdentity);

            return properties;
        }
    }
}
