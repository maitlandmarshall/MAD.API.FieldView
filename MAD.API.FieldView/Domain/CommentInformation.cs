using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.API.FieldView.Domain
{
    public class CommentInformation
    {
        public string FormId { get; set; }
        public string FormAnswerId { get; set; }

        public string Comment { get; set; }
        public string CreatedByUser { get; set; }
        public string CreatedByOrganisation { get; set; }

        public string OwnerId { get; set; }
        public string CommentType { get; set; }

        public DateTime? Date { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
