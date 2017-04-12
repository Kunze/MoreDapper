using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MoreDapper.Config
{
    public static class MoreDapperConfig
    {
        private static HashSet<string> GlobalPrimaryKeys = new HashSet<string>
        {
            "Id"
        };

        private static Dictionary<Type, HashSet<string>> PrimaryKeys = new Dictionary<Type, HashSet<string>>();

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
            if (property == null)
            {
                throw new ArgumentNullException("property can not be null.");
            }

            GlobalPrimaryKeys.Add(property);
        }

        public static void AddPrimaryKey(Type type, string property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property can not be null.");
            }

            if (string.IsNullOrWhiteSpace(property))
            {
                throw new ArgumentNullException("property can not be null or white space.");
            }

            var typeHasProperty = type.GetProperty(property);

            if (typeHasProperty == null)
            {
                throw new ArgumentException($"type '{type.Name}' does not have a property '{property}'.");
            }

            var typeProperties = new HashSet<string>();
            if (PrimaryKeys.TryGetValue(type, out typeProperties))
            {
                typeProperties.Add(property);
            }
            else
            {
                PrimaryKeys.Add(type, new HashSet<string>
                {
                    property
                });
            }
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

            foreach (var property in properties)
            {
                AddPrimaryKey(type, properties);
            }
        }

        public static void AddPrimaryKey<TSource, TProperty>(Expression<Func<TSource, TProperty>> property)
        {
            var type = typeof(TSource);
            var expression = (MemberExpression)property.Body;

            AddPrimaryKey(type, expression.Member.Name);
        }

        public static HashSet<string> GetKeysFor(Type type)
        {
            var properties = new HashSet<string>();
            if (!PrimaryKeys.TryGetValue(type, out properties))
            {
                properties = new HashSet<string>();
            }
            foreach (var property in GlobalPrimaryKeys)
            {
                properties.Add(property);
            }

            return properties;
        }
    }
}
