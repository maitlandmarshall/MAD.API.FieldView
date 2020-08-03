using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.API.FieldView.Domain
{
    public class FormPdf
    {
        public string FormId { get; set; }
        public byte[] FilePayload { get; set; }
    }
}
