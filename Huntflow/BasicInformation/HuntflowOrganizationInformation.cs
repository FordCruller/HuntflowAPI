using Newtonsoft.Json;
using System;

namespace HuntflowAPI
{
    public class HuntflowOrganizationInformation
    {
        public int id { get; set; }
        public string name { get; set; }
        public string nick { get; set; }
        public string locale { get; set; }
        public Uri photo { get; set; }

        public HuntflowOrganizationInformation(string json)
        {
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }
}
