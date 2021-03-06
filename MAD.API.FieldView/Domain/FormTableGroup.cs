﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.API.FieldView.Domain
{
    public class FormTableGroup
    {
        public string FormId { get; set; } 
        public int FormTemplateId { get; set; }

        public string TableGroupAlias { get; set; }
        public string RowAlias { get; set; }

        public List<FormTableGroupQuestion> Questions { get; set; }
        public List<FormTableGroupAnswer> Answers { get; set; }
    }
}
