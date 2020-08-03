using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MAD.API.FieldView.UnitTests
{
    public static class ClientFactory
    {
        public static FieldViewClient Create()
        {
            string token = File.ReadAllText("APIToken.txt");

            FieldViewClient client = new FieldViewClient(token);

            return client;
        }
    }
}
