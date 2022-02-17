using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.API.FieldView.Domain
{
    public class SimplePredefinedAnswerGroupItemInformation
    {
        public int PredefinedAnswerId { get; set; }
        public string Description { get; set; }
        public string Colour { get; set; }
        public int? ParentId { get; set; }

        public int PredefinedAnswerGroupId { get; set; }

        public double? Weight { get; set; }
        public double? Score { get; set; }
        public int? ProjectId { get; set; }
        public string Active { get; set; }
        public double? SortOrder { get; set; }
    }
}
