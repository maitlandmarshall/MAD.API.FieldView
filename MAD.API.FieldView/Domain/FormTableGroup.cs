using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.API.FieldView.Domain
{
    public class FormTableGroup
    {
        public string FormId { get; set; } 
        public int FormTemplateLinkId { get; set; }

        public List<FormTableGroupQuestion> Questions { get; set; }
        public List<FormTableGroupAnswer> Answers { get; set; }
    }
}
