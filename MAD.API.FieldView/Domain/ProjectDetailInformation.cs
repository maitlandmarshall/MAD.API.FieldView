using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.API.FieldView.Domain
{
    public class ProjectDetailInformation
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
        public string AddressType { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public bool Calibration { get; set; }
        public string CC { get; set; }
        public int? PostCode { get; set; }
        public bool ViewAllForms { get; set; }
        public bool ViewOriginalTasks { get; set; }
        public bool StampPhotosWithDate { get; set; }
        public bool StampPhotosWithGPS { get; set; }
        public string ClientProjectReference { get; set; }
        public string Notes { get; set; }
    }
}
