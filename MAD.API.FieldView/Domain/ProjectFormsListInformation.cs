using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.API.FieldView.Domain
{
    public class ProjectFormsListInformation
    {
        public string FormId { get; set; }
        public int FormTemplateLinkId { get; set; }
        public bool Deleted { get; set; }
        public string FormType { get; set; }
        public string FormName { get; set; }
        public string FormTitle { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string OwnedBy { get; set; }
        public string OwnedByOrganisation { get; set; }
        public string IssuedToOrganisation { get; set; }
        public string Status { get; set; }
        public string StatusColour { get; set; }
        public DateTime? StatusDate { get; set; }
        public string Location { get; set; }
        public int OpenTasks { get; set; }
        public int ClosedTasks { get; set; }
        public DateTime? FormExpiryDate { get; set; }
        public bool OverDue { get; set; }
        public bool Complete { get; set; }
        public bool Closed { get; set; }
        public string ParentFormId { get; set; }
        public DateTime? LastModified { get; set; }
        public DateTime? LastModifiedOnServer { get; set; }
        public string ClosedBy { get; set; }
        public int FormTemplateId { get; set; }
    }
}
