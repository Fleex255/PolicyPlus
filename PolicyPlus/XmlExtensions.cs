using System.ComponentModel;
using System.Xml;

namespace PolicyPlus
{
    public static class XmlExtensions
    {
        // Convenience methods for parsing XML in AdmxFile and AdmlFile
        public static string AttributeOrNull(this XmlNode Node, string Attribute)
        {
            if (Node.Attributes[Attribute] is null)
                return null;
            else
                return Node.Attributes[Attribute].Value;
        }
        public static object AttributeOrDefault(this XmlNode Node, string Attribute, object DefaultVal)
        {
            if (Node.Attributes[Attribute] is null)
                return DefaultVal;
            var converter = TypeDescriptor.GetConverter(DefaultVal.GetType());
            if (converter.IsValid(Node.Attributes[Attribute].Value))
            {
                return converter.ConvertFromString(Node.Attributes[Attribute].Value);
            }
            return DefaultVal;
        }
    }
}