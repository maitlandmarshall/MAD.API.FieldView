namespace MAD.API.FieldView.Domain
{
    public class FormTableGroupQuestion
    {
        public int FormTemplateId { get; set; }
        public string QuestionType { get; set; }
        public string DataType { get; set; }
        public string Question { get; set; }
        public string Alias { get; set; }
        public int SortOrder { get; set; }
    }
}