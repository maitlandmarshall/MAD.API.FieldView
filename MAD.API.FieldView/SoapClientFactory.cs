using ConfigurationServicesEndpoint;
using FormsServicesEndpoint;
using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.Text;

namespace MAD.API.FieldView
{
    internal class SoapClientFactory
    {
        private void SetBindingTimeouts(Binding binding)
        {
            binding.CloseTimeout = TimeSpan.FromMinutes(500);
            binding.OpenTimeout = TimeSpan.FromMinutes(500);
            binding.ReceiveTimeout = TimeSpan.FromMinutes(500);
            binding.SendTimeout = TimeSpan.FromMinutes(500);
        }

        public API_FormsServicesSoapClient CreateFormsServicesClient(string baseUri = "")
        {
            API_FormsServicesSoapClient result;

            if (!string.IsNullOrEmpty(baseUri))
            {
                result = new API_FormsServicesSoapClient(API_FormsServicesSoapClient.EndpointConfiguration.API_FormsServicesSoap, baseUri + "API_FormsServices.asmx");
            }
            else
            {
                result = new API_FormsServicesSoapClient(API_FormsServicesSoapClient.EndpointConfiguration.API_FormsServicesSoap);
            }

            this.SetBindingTimeouts(result.Endpoint.Binding);

            return result;
        }

        public API_ConfigurationServicesSoapClient CreateConfigurationServicesClient(string baseUri = "")
        {
            API_ConfigurationServicesSoapClient result;

            if (!string.IsNullOrEmpty(baseUri))
            {
                result = new API_ConfigurationServicesSoapClient(API_ConfigurationServicesSoapClient.EndpointConfiguration.API_ConfigurationServicesSoap, baseUri + "API_ConfigurationServices.asmx");
            }
            else
            {
                result = new API_ConfigurationServicesSoapClient(API_ConfigurationServicesSoapClient.EndpointConfiguration.API_ConfigurationServicesSoap);
            }

            this.SetBindingTimeouts(result.Endpoint.Binding);

            return result;
        }
    }
}
