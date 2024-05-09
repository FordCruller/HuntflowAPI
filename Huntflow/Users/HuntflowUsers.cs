using Newtonsoft.Json;
using System.Collections.Generic;

namespace HuntflowAPI
{
    public class HuntflowUsers
    {
        public int page { get; set; }
        public int count { get; set; }
        public int total_pages { get; set; }
        public int total_items { get; set; }
        public HuntflowUser[] items { get; set; }

        public class HuntflowUser
        {
            public int id { get; set; }
            public string name { get; set; }
            public string position { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public string locale { get; set; }
            public string meta { get; set; }
            public GetRequest.UserType type { get; set; }
            public string head_id { get; set; }
            public string[] division_ids { get; set; }
            public Permission[] permissions { get; set; }

            public class Permission
            {
                public string permission { get; set; }
                public string value { get; set; }
                public int vacancy { get; set; }
            }

            public HuntflowUser(string json)
            {
                JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
            }
        }

        public HuntflowUsers() { }

        public HuntflowUsers(string json)
        {
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }
}