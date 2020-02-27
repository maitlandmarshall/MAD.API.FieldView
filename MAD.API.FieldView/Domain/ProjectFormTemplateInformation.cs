using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.API.FieldView.Domain
{
    public class ProjectFormTemplateInformation
    {
        public int FormTemplateId { get; set; }
        public int FormTemplateLinkId { get; set; }
        public string FormTemplate { get; set; }
        public int TypeId { get; set; }
        public string Type { get; set; }
        public int WorkFlowTemplateId { get; set; }
        public string WorkFlowTemplate { get; set; }
        public int BusinessUnitId { get; set; }
        public string BusinessUnit { get; set; }
        public bool Active { get; set; }
        public int ProjectId { get; set; }
    }
}
