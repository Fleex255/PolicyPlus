using System.ComponentModel;
using System.Xml;

namespace PolicyPlus.csharp.Helpers
{
    public static class XmlExtensions
    {
        // Convenience methods for parsing XML in AdmxFile and AdmlFile
        public static string? AttributeOrNull(this XmlNode node, string attribute) => node?.Attributes?[attribute] is null ? null : node.Attributes[attribute].Value;

        public static object AttributeOrDefault(this XmlNode node, string attribute, object defaultVal)
        {
            if (node.Attributes?[attribute] is null)
            {
                return defaultVal;
            }

            var converter = TypeDescriptor.GetConverter(defaultVal.GetType());
            if (converter == null)
            {
                return defaultVal;
            }

            var obj = node?.Attributes?[attribute]?.Value;
            if (obj == null)
            {
                return defaultVal;
            }

            if (!converter.IsValid(obj))
            {
                return defaultVal;
            }
            var newVal = converter.ConvertFromString(obj);

            return newVal ?? defaultVal;
        }
    }
}