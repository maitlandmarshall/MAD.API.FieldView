using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.API.FieldView.Domain
{
    public class FormWorkflowStatus
    {
        public string Id { get; set; }
        public string Description { get; set; }

        public int FormTemplateId { get; set; }
    }
}
