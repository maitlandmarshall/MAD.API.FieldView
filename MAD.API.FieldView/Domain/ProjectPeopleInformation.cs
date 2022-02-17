using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.API.FieldView.Domain
{
    public class ProjectPeopleInformation
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public int OrganisationId { get; set; }
        public string Organisation { get; set; }

        public int OrganisationTypeId { get; set; }
        public string OrganisationType { get; set; }

        public string LoginName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string MobilePhone { get; set; }
        public string Fax { get; set; }

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalInformation { get; set; } = new Dictionary<string, object>();
    }
}
