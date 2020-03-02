using MAD.API.FieldView.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.API.FieldView
{
    internal class FieldViewResponseFactory
    {
        public FieldViewResponse<TEntity> Create<TEntity>(string json)
        {
            FieldViewResponse<TEntity> response = JsonConvert.DeserializeObject<FieldViewResponse<TEntity>>(json);

            // Should be an object like this { "ProjectDetailInformation" : [] }
            JObject result = JsonConvert.DeserializeObject<JObject>(json);
            response.Entities = result.First.First.ToObject<IEnumerable<TEntity>>();

            return response;
        }

        public FieldViewFormTableGroupResponse Create (string json)
        {
            // TODO: Is there a better way to do this? I wish the API was more consistent with its responses
            FieldViewFormTableGroupResponse response = JsonConvert.DeserializeObject<FieldViewFormTableGroupResponse>(json);

            return response;
        }


    }
}
