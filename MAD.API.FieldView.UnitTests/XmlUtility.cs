using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace MAD.API.FieldView.UnitTests
{
    [TestClass]
    public class XmlUtility
    {
        [DataTestMethod]
        [DataRow(@"
  <FormTemplateDetailsInformation>
	    <FormTemplateId></FormTemplateId>
	    <ParentId></ParentId>
	    <ShortQuestion></ShortQuestion>
	    <LongQuestion></LongQuestion>
	    <SortOrder></SortOrder>
	    <Level></Level>
	    <NodeType></NodeType>
<QuestionType></QuestionType>
</FormTemplateDetailsInformation>
")]
        public void XmlStringToClass(string xml)
        {
			XDocument xmlReader = XDocument.Parse(xml);
			IEnumerable<XElement> elements = xmlReader.Elements().Elements();

			List<string> builtProperties = new List<string>();

			foreach (var e in elements)
			{
				string propName = e.Name.LocalName;

				if (propName.Contains("ID"))
					propName = propName.Replace("ID", "Id");

				string valueType;

				if (bool.TryParse(e.Value, out bool _))
					valueType = "bool";
				else if (int.TryParse(e.Value, out int _))
					valueType = "int";
				else if (double.TryParse(e.Value, out double _))
					valueType = "double";
				else if (propName.Contains("Date"))
					valueType = "DateTime?";
				else
					valueType = "string";

				builtProperties.Add($"public {valueType} {propName} {{ get; set; }}");
			}

			string result = String.Join(Environment.NewLine, builtProperties);
        }
    }
}
