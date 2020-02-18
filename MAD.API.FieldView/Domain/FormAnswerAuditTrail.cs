using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.API.FieldView.Domain
{
    public class FormAnswerAuditTrail
    {
        public string FormId { get; set; }
        public string FormAnswerId { get; set; }
        public int FormTemplateId { get; set; }
        public string Action { get; set; }
        public string AnswerText { get; set; }
        public DateTime? DateChanged { get; set; }
        public string Person { get; set; }
        public string Device { get; set; }
    }
}
