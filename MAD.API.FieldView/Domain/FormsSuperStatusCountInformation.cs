using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.API.FieldView.Domain
{
    public class FormsSuperStatusCountInformation
    {
        public string Project { get; set; }
        public string TaskType { get; set; }
        public string FormTemplate { get; set; }
        public int Opened { get; set; }
        public int Completed { get; set; }
        public int Closed { get; set; }
    }
}
