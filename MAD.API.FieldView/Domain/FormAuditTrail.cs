using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.API.FieldView.Domain
{
    public class FormAuditTrail
    {
        public string FormId { get; set; }

        public string WorkflowId { get; set; }
        public string Status { get; set; }
        public string StatusColor { get; set; }
        public DateTime? Date { get; set; }
        public string SignedBy { get; set; }
        public string Organisation { get; set; }
        public string SignatureType { get; set; }
        public string ClosedStatus { get; set; }
        public string LoggedOnUser { get; set; }
    }
}
