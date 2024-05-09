using System;
using Newtonsoft.Json;

namespace HuntflowAPI
{
    public class HuntflowRejectionReasons
    {
        public HuntflowRejectionReason[] items { get; set; }

        public class HuntflowRejectionReason
        {
            public int id { get; set; }
            public string name { get; set; }
            public int order { get; set; }
        }

        public HuntflowRejectionReasons()
        {
            items = new HuntflowRejectionReason[0];
        }

        public HuntflowRejectionReasons(string json)
        {
            items = new HuntflowRejectionReason[0];
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }
}