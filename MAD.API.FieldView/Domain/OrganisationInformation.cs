using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.API.FieldView.Domain
{
    public class OrganisationInformation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string RegistrationNo { get; set; }
        public bool Active { get; set; }
    }
}
