using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.API.FieldView.Domain
{
    public class BusinessUnitInformation
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int OrganisationId { get; set; }
        public int BusinessUnitId { get; set; }
        public string Name { get; set; }
    }
}
