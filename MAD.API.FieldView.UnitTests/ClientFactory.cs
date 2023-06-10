using System.IO;

namespace MAD.API.FieldView.UnitTests
{
    public static class ClientFactory
    {
        public static FieldViewClient Create()
        {
            var token = File.ReadAllText("APIToken.txt");
            var client = new FieldViewClient(token, FieldViewDataCenter.AU.GetUri());

            return client;
        }
    }
}
