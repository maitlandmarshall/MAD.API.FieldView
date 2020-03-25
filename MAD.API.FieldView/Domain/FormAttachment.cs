using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.API.FieldView.Domain
{
    public class FormAttachment
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string MediaType { get; set; }
        public string OwnerId { get; set; }

        public string FormId { get; set; }
    }
}
