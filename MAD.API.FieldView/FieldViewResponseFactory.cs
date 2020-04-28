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

            if (typeof(FormPhoto).IsAssignableFrom(typeof(TEntity))
                || typeof(FormDocument).IsAssignableFrom(typeof(TEntity)))
            {
                // The API just doesn't have a consistent response pattern. These two entities's properties are returned in the root object
                // rather than being a nested object property.

                response.Entities = new List<TEntity>
                {
                    JsonConvert.DeserializeObject<TEntity>(json)
                };
            }
            else
            {
                // Should be an object like this { "ProjectDetailInformation" : [] }
                JObject result = JsonConvert.DeserializeObject<JObject>(json);
                response.Entities = result.First.First.ToObject<IEnumerable<TEntity>>();
            }

            if (response.Status.Code != 2)
                throw new FieldViewResponseException(response);

            return response;
        }

        public FieldViewFormTableGroupResponse Create (string json)
        {
            // TODO: Is there a better way to do this? I wish the API was more consistent with its responses
            FieldViewFormTableGroupResponse response = JsonConvert.DeserializeObject<FieldViewFormTableGroupResponse>(json);

            if (response.Status.Code != 2)
                throw new FieldViewResponseException(response);

            return response;
        }


    }
}
