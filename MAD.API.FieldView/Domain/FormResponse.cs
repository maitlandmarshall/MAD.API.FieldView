using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.API.FieldView.Domain
{
    public class FormResponse
    {
        public int? Id { get; set; }
        public FormResponseStatus Status { get; set; }
    }

    public class FormResponseStatus
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}
