using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HuntflowAPI
{
    public class HuntflowUserEmailAccountList
    {
        public List<HuntflowUserEmailAccount> items { get; set; }

        public HuntflowUserEmailAccountList()
        {
            items = new List<HuntflowUserEmailAccount>();
        }

        public class HuntflowUserEmailAccount
        {
            public int id { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public bool receive { get; set; }
            public bool send { get; set; }
            public DateTime? last_sync { get; set; }
        }

        public HuntflowUserEmailAccountList(string json)
        {
            items = new List<HuntflowUserEmailAccount>();
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }
}