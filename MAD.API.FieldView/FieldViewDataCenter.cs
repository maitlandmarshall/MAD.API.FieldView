using System;

namespace MAD.API.FieldView
{
    public enum FieldViewDataCenter
    {
        AU,
        UK,
        NA
    }

    public static class FieldViewDataCenterExtensions
    {
        public static string GetUri(this FieldViewDataCenter dataCenter)
        {
            switch (dataCenter)
            {
                case FieldViewDataCenter.AU:
                    return "https://anz.fieldview.viewpoint.com/FieldViewWebServices/WebServices/JSON/";
                case FieldViewDataCenter.NA:
                    return "https://us.fieldview.viewpoint.com/FieldViewWebServices/WebServices/JSON/";
                case FieldViewDataCenter.UK:
                    return "https://www.priority1.uk.net/FieldViewWebServices/WebServices/JSON/";
                default:
                    throw new NotImplementedException();
            }
        }
    }
}