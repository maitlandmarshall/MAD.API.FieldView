using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.API.FieldView
{
    public class FieldViewResponseException : Exception
    {
        public int StatusCode { get; set; }

        internal FieldViewResponseException(FieldViewResponse errorResponse) : base($"Error Code {errorResponse.Status.Code}: {errorResponse.Status.Message}")
        {
            this.StatusCode = errorResponse.Status.Code;
        }
    }
}
