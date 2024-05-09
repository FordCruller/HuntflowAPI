using System.Collections.Generic;
using Newtonsoft.Json;

namespace HuntflowAPI
{
    public class HuntflowCoworker
    {
        public int id { get; set; }
        public int member { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int head { get; set; }
        public string email { get; set; }
        public string meta { get; set; }
        public Permission[] permissions { get; set; }

        public class Permission
        {
            public string permission { get; set; }
            public string value { get; set; }
            public int vacancy { get; set; }
        }

        public HuntflowCoworker() { }

        public HuntflowCoworker(string json)
        {
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }

    public class HuntflowCoworkers
    {
        public int page { get; set; }
        public int count { get; set; }
        public int total_pages { get; set; }
        public int total_items { get; set; }
        public List<HuntflowCoworker> items { get; set; }

        public HuntflowCoworkers() { }

        public HuntflowCoworkers(string json)
        {
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }
}