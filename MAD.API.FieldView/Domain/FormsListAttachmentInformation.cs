using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.API.FieldView.Domain
{
    public class FormsListAttachmentInformation
    {
        public string Id { get; set; }
        public string FormId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string OwnerId { get; set; }
        public string OwnerType { get; set; }
        public DateTime? LastModified { get; set; }
        public DateTime? LastModifiedOnServer { get; set; }
    }
}
