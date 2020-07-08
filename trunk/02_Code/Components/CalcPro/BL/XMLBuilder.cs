using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BL
{
    public class XMLBuilder
    {
        /// <summary>
        /// Code to add an attribute in XML file
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <param name="xpath"></param>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static XmlDocument XmlConstruct(XmlDocument xmlDocument, string xpath, string Name, string Value)
        {
            try
            {
                XmlNode parentNode = xmlDocument;
                xpath += "/" + Name;
                if (xmlDocument != null && !string.IsNullOrEmpty(xpath))
                {
                    string[] partsOfXPath = xpath.Split('/');
                    string xPathSoFar = string.Empty;
                    foreach (string xPathElement in partsOfXPath)
                    {
                        if (string.IsNullOrEmpty(xPathElement))
                            continue;
                        xPathSoFar += "/" + xPathElement.Trim();
                        XmlNode childNode = xmlDocument.SelectSingleNode(xPathSoFar);
                        if (childNode == null)
                        {
                            childNode = xmlDocument.CreateElement(xPathElement);
                        }
                        parentNode.AppendChild(childNode);
                        parentNode = childNode;
                    }
                    XmlText text1 = xmlDocument.CreateTextNode(Value);
                    parentNode.AppendChild(text1);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return xmlDocument;
        }

    }
}
