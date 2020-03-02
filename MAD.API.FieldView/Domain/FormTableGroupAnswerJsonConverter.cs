using MAD.API.FieldView.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MAD.API.FieldView.Domain
{
    internal class FormTableGroupAnswerJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(FormTableGroupAnswerJsonConverter);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jResult = JObject.Load(reader);
            string[] propNamesToExclude = typeof(FormTableGroupAnswer).GetProperties().Select(y => y.Name.ToLower()).ToArray();

            FormTableGroupAnswer result = jResult.ToObject<FormTableGroupAnswer>(new JsonSerializer());
            result.CustomAnswers = new JObject(jResult.Properties().Where(y => !propNamesToExclude.Contains(y.Name.ToLower())));

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
             writer.WriteRaw(JsonConvert.SerializeObject(value));
        }
    }
}