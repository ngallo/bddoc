using System;
using System.Linq;
using System.Xml.Linq;

namespace BDDoc.Core
{
    internal class BDDocXmlHelper
    {
        //Methods

        internal static XElement GetStoryElement(XContainer document)
        {
            if (document == null)
            {
                throw new ArgumentNullException();
            }
            var xElements = document.Elements();
            var enumerable = xElements as XElement[] ?? xElements.ToArray();
            return (from element in enumerable
                    where element.Name == BDDocXmlConstants.CStoryElement
                    select element).FirstOrDefault();
        }

        internal static XElement GetItemsElement(XContainer element)
        {
            if (element == null)
            {
                throw new ArgumentNullException();
            }
            return element.Elements().FirstOrDefault(xElement => xElement.Name == BDDocXmlConstants.CItemElementCollection);
        }

        internal static string GetFileRelativePath(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException();
            }
            return string.Format("{0}.{1}", fileName, BDDocXmlConstants.CBDDocFileExtension);
        }
    }
}
