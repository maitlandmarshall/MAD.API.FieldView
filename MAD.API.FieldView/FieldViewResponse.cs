﻿using MAD.API.FieldView.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.API.FieldView
{
    internal class FieldViewResponse
    {
        public class FieldViewStatus
        {
            public int Code { get; set; }
            public string Message { get; set; }
        }

        public FieldViewStatus Status { get;set; }
    }

    internal class FieldViewResponse <TEntity> : FieldViewResponse
    {
        public IEnumerable<TEntity> Entities { get; set; }
    }

    internal class FieldViewFormTableGroupResponse : FieldViewResponse
    {
        public List<FormTableGroupQuestion> Questions { get; set; }

        [JsonProperty(ItemConverterType = typeof(FormTableGroupAnswerJsonConverter))]
        public List<FormTableGroupAnswer> Answers { get; set; }
    }
}
