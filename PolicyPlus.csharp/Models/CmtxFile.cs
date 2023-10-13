using System.Collections.Generic;
using System.Xml;
using PolicyPlus.csharp.Helpers;

namespace PolicyPlus.csharp.Models
{
    public class CmtxFile
    {
        public Dictionary<string, string> Prefixes { get; } = new();
        public Dictionary<string, string> Comments { get; } = new();
        public Dictionary<string, string> Strings { get; } = new();
        public string SourceFile { get; set; }

        public static CmtxFile Load(string file)
        {
            // CMTX documentation: https://msdn.microsoft.com/en-us/library/dn605929(v=vs.85).aspx
            var cmtx = new CmtxFile
            {
                SourceFile = file
            };
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(file);
            var policyComments = xmlDoc.GetElementsByTagName("policyComments")[0];
            foreach (XmlNode child in policyComments.ChildNodes)
            {
                switch (child.LocalName ?? "")
                {
                    case "policyNamespaces": // ADMX file prefixes
                        {
                            foreach (XmlNode usingElement in child.ChildNodes)
                            {
                                if (usingElement.LocalName != "using")
                                {
                                    continue;
                                }

                                var prefix = usingElement.AttributeOrNull("prefix");
                                var ns = usingElement.AttributeOrNull("namespace");
                                cmtx.Prefixes.Add(prefix, ns);
                            }

                            break;
                        }
                    case "comments": // Policy to comment ID mapping
                        {
                            foreach (XmlNode admTemplateElement in child.ChildNodes)
                            {
                                if (admTemplateElement.LocalName != "admTemplate")
                                {
                                    continue;
                                }

                                foreach (XmlNode commentElement in admTemplateElement.ChildNodes)
                                {
                                    if (commentElement.LocalName != "comment")
                                    {
                                        continue;
                                    }

                                    var policy = commentElement.AttributeOrNull("policyRef");
                                    var text = commentElement.AttributeOrNull("commentText");
                                    cmtx.Comments.Add(policy, text);
                                }
                            }

                            break;
                        }
                    case "resources": // The actual comment text
                        {
                            foreach (XmlNode stringTable in child.ChildNodes)
                            {
                                if (stringTable.LocalName != "stringTable")
                                {
                                    continue;
                                }

                                foreach (XmlNode stringElement in stringTable.ChildNodes)
                                {
                                    if (stringElement.LocalName != "string")
                                    {
                                        continue;
                                    }

                                    var id = stringElement.AttributeOrNull("id");
                                    var text = stringElement.InnerText;
                                    cmtx.Strings.Add(id, text);
                                }
                            }

                            break;
                        }
                }
            }
            return cmtx;
        }

        public static CmtxFile FromCommentTable(Dictionary<string, string> table)
        {
            // Create CMTX structures from a simple policy-to-comment-text mapping
            var cmtx = new CmtxFile();
            var resNum = 0; // A counter to make sure names are unique
            var revPrefixes = new Dictionary<string, string>(); // Opposite-direction prefix lookup
            foreach (var kv in table)
            {
                var idParts = kv.Key.Split(":", 2);
                if (!revPrefixes.ContainsKey(idParts[0]))
                {
                    var prefixId = idParts[0].Replace('.', '_') + "__" + resNum;
                    revPrefixes.Add(idParts[0], prefixId);
                    cmtx.Prefixes.Add(prefixId, idParts[0]);
                }
                var resourceId = idParts[0].Replace('.', '_') + "__" + idParts[1] + "__" + resNum;
                cmtx.Strings.Add(resourceId, kv.Value);
                cmtx.Comments.Add(revPrefixes[idParts[0]] + ":" + idParts[1], "$(resource." + resourceId + ")");
                resNum++;
            }
            return cmtx;
        }

        public Dictionary<string, string> ToCommentTable()
        {
            // Create a convenient policy-to-comment-text mapping
            var commentTable = new Dictionary<string, string>();
            foreach (var comment in Comments)
            {
                var refParts = comment.Key.Split(":", 2);
                var polNamespace = Prefixes[refParts[0]];
                var stringRef = comment.Value;
                var stringId = stringRef.Substring(11, stringRef.Length - 12); // "$(resource." is 11 characters long
                commentTable.Add(polNamespace + ":" + refParts[1], Strings[stringId]);
            }
            return commentTable;
        }

        public void Save() => Save(SourceFile);

        public void Save(string file)
        {
            // Save the CMTX data to a fully compliant XML document
            var xml = new XmlDocument();
            var declaration = xml.CreateXmlDeclaration("1.0", "utf-8", "");
            _ = xml.AppendChild(declaration);
            var policyComments = xml.CreateElement("policyComments");
            policyComments.SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
            policyComments.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            policyComments.SetAttribute("revision", "1.0");
            policyComments.SetAttribute("schemaVersion", "1.0");
            policyComments.SetAttribute("xmlns", "http://www.microsoft.com/GroupPolicy/CommentDefinitions");
            var policyNamespaces = xml.CreateElement("policyNamespaces");
            foreach (var prefix in Prefixes)
            {
                var usingElem = xml.CreateElement("using");
                usingElem.SetAttribute("prefix", prefix.Key);
                usingElem.SetAttribute("namespace", prefix.Value);
                _ = policyNamespaces.AppendChild(usingElem);
            }
            _ = policyComments.AppendChild(policyNamespaces);
            var commentsElem = xml.CreateElement("comments");
            var admTemplate = xml.CreateElement("admTemplate");
            foreach (var comment in Comments)
            {
                var commentElem = xml.CreateElement("comment");
                commentElem.SetAttribute("policyRef", comment.Key);
                commentElem.SetAttribute("commentText", comment.Value);
                _ = admTemplate.AppendChild(commentElem);
            }
            _ = commentsElem.AppendChild(admTemplate);
            _ = policyComments.AppendChild(commentsElem);
            var resources = xml.CreateElement("resources");
            resources.SetAttribute("minRequiredRevision", "1.0");
            var stringTable = xml.CreateElement("stringTable");
            foreach (var textval in Strings)
            {
                var stringElem = xml.CreateElement("string");
                stringElem.SetAttribute("id", textval.Key);
                stringElem.InnerText = textval.Value;
                _ = stringTable.AppendChild(stringElem);
            }
            _ = resources.AppendChild(stringTable);
            _ = policyComments.AppendChild(resources);
            _ = xml.AppendChild(policyComments);
            xml.Save(file);
        }
    }
}