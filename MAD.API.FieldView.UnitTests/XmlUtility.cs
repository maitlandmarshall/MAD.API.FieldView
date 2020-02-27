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
  <ProjectInformation>
           <ID></ID>
           <Name></Name>
           <Reference></Reference>
           <ProjectOwnerID></ProjectOwnerID>
           <ProjectOwner></ProjectOwner>
           <BusinessUnitTypeID></BusinessUnitTypeID>
           <BusinessUnitType></BusinessUnitType>
           <ProjectTypeID></ProjectTypeID>
           <ProjectType></ProjectType>
           <StartDate></StartDate>
           <FinishDate></FinishDate>
           <TimeZoneOffset></TimeZoneOffset>
           <CultureID></CultureID>
           <Culture></Culture>
           <ResolutionDays></ResolutionDays>
           <Active></Active>
       </ProjectInformation>
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
