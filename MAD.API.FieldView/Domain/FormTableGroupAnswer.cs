using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MAD.API.FieldView.Domain
{
    public class FormTableGroupAnswer
    {
        public string FormId { get; set; }

        public string Id { get; set; }
        public string FormAnswerLinkId { get; set; }
        public DateTime? FormAnswerLinkDate { get; set; }
        public int SortOrder { get; set; }
        public string CheckItem { get; set; }
        public string Answers { get; set; }
        public string AnsweredBy { get; set; }

        public DateTime? AnsweredDateTime { get; set; }
        public bool HasActions { get; set; }
        public bool HasImages { get; set; }
        public bool HasComments { get; set; }
        public bool HasDocuments { get; set; }

        public JObject CustomAnswers { get; set; }

        public int FormTemplateId { get; set; }
    }
}