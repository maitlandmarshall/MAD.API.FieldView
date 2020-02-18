using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.API.FieldView.Domain
{
    public class ProjectFormsListUpdatedInformation
    {
        public int FormTemplateId { get; set; }
        public string FormId { get; set; }
        public string FormAnswerId { get; set; }
        public string FormAnswerLinkId { get; set; }
        public string QuestionType { get; set; }
        public string DataType { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Alias { get; set; }
        public int SortOrder { get; set; }
        public string AnsweredBy { get; set; }
        public DateTime? AnsweredDateTime { get; set; }
        public bool IsTableGroup { get; set; }
        public bool Deleted { get; set; }
        public bool HasActions { get; set; }
        public bool HasImages { get; set; }
        public bool HasComments { get; set; }
        public bool HasDocuments { get; set; }
        public DateTime? AnswerLastModifiedOnServer { get; set; }
    }
}
