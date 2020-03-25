using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.API.FieldView.Domain
{
    public class FormPhoto
    {
        public string FormId { get; set; }

        public string MediaId { get; set; }
        public string Media { get; set; }
        public DateTime? DateRecorded { get; set; }
    }
}
