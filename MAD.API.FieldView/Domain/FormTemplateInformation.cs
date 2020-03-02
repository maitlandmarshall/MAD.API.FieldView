using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.API.FieldView.Domain
{
    public class FormTemplateInformation
    {
        public int FormTemplateId { get; set; }
        public int ParentId { get; set; }
        public string ShortQuestion { get; set; }
        public string LongQuestion { get; set; }
        public int SortOrder { get; set; }
        public int Level { get; set; }
        public string NodeType { get; set; }
        public string QuestionType { get; set; }
    }
}
