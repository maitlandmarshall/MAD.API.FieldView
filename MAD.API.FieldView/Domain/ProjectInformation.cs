using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.API.FieldView.Domain
{
    public class ProjectInformation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Reference { get; set; }
        public int ProjectOwnerId { get; set; }
        public string ProjectOwner { get; set; }
        public int BusinessUnitTypeId { get; set; }
        public string BusinessUnitType { get; set; }
        public int ProjectTypeId { get; set; }
        public string ProjectType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public int TimeZoneOffset { get; set; }
        public int CultureId { get; set; }
        public string Culture { get; set; }
        public int ResolutionDays { get; set; }
        public bool Active { get; set; }
    }
}
