using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.API.FieldView.Domain
{
    public class SimplePredefinedAnswerGroupInformation
    {
        public int PredefinedAnswerGroupId { get; set; }
        public string Description { get; set; }
        public bool Measurement { get; set; }
        public bool ProjectBasedAnswers { get; set; }

        public int OrganisationId { get; set; }
        public string Organisation { get; set; }
        public bool AllowOther { get; set; }
        public bool OrganisationUnitAndBelow { get; set; }
        public int VisibilityOrganisationId { get; set; }
        public string VisibilityOrganisation { get; set; }
        public bool Active { get; set; }
    }
}
