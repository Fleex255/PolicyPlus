using System.ComponentModel;
using System.Xml;

namespace PolicyPlus.Helpers
{
    public static class XmlExtensions
    {
        // Convenience methods for parsing XML in AdmxFile and AdmlFile
        public static string AttributeOrNull(this XmlNode node, string attribute) => node.Attributes?[attribute] is null ? null : node.Attributes[attribute].Value;

        public static object AttributeOrDefault(this XmlNode node, string attribute, object defaultVal)
        {
            if (node.Attributes?[attribute] is null)
            {
                return defaultVal;
            }

            var converter = TypeDescriptor.GetConverter(defaultVal.GetType());
            return converter.IsValid(node.Attributes[attribute].Value) ? converter.ConvertFromString(node.Attributes[attribute].Value) : defaultVal;
        }
    }
}