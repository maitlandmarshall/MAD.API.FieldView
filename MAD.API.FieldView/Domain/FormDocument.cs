using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.API.FieldView.Domain
{
    public class FormDocument
    {
        public string FormId { get; set; }

        public int DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentType { get; set; }
        public byte[] DocumentFile { get; set; }
        public DateTime? Date { get; set; }

        public string Forename { get; set; }
        public string Surname { get; set; }
    }
}
