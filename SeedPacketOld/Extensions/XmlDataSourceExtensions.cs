using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace SeedPacket.Extensions
{
    // TODO  - MOVE TO WILDHARE EXTENSIONS
    public static class XmlDataSourceExtensions
    {
        public static T ToObject<T>(this XElement element) where T : class, new()
        {
            var instance = new T();
            var typeProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance); //.GetMetaProperties();

            foreach (var property in typeProperties)
            {
                var xattribute = element.Attribute(property.Name);
                var xelement = element.Element(property.Name);
                var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                if (xattribute == null && xelement == null)
                {
                    if (property.CustomAttributes.Where(a => a.AttributeType.Name == "XmlIgnoreAttribute").Count() == 0)
                    {
                        string message = $"Unable to parse XML value. Object '{element.Name}' does not contain either an attribute or an element called '{property.Name}'.";
                        Debug.WriteLine(message);
                    }
                    continue;
                }

                var value = xattribute?.Value ?? xelement.Value;
                try
                {
                    if (value != null)
                    {
                        if (property.CanWrite)
                        {
                            property.SetValue(instance, Convert.ChangeType(value, propertyType));
                        }
                    }
                }
                catch (Exception ex) // If Error let the value remain default for that property type
                {
                    Debug.WriteLine($"Not able to parse XML value {value} for type '{property.PropertyType}' " +
                                     $"for property {property.Name}. Ex: {ex.Message}");
                }
            }

            if (instance == null)
                throw new Exception($"Not able to parse XML {element.Name}");

            return instance;
        }

    }
}
