using Newtonsoft.Json;
using System;

namespace HuntflowAPI
{
    public class HuntflowApplicants
    {
        public int page { get; set; }
        public int count { get; set; }
        public int total_pages { get; set; }
        public int total_items { get; set; }
        public HuntflowApplicant[] items { get; set; }

        public HuntflowApplicants()
        {
            items = new HuntflowApplicant[0];
        }

        public HuntflowApplicants(string json)
        {
            items = new HuntflowApplicant[0];
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }

        public HuntflowApplicants(HuntflowApplicant[] items)
        {
            page = 1;
            count = items.Length;
            total_pages = 1;
            total_items = items.Length;

            this.items = items;
        }
    }

    public class HuntflowApplicant
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string middle_name { get; set; }
        public string money { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string skype { get; set; }
        public string position { get; set; }
        public string company { get; set; }
        public int photo { get; set; }
        public int id { get; set; }
        public int account { get; set; }
        public string photo_url { get; set; }
        public DateTime birthday { get; set; }
        public DateTime created { get; set; }
        public Tag[] tags { get; set; }
        public Link[] links { get; set; }
        public External[] external { get; set; }
        public Agreement agreement { get; set; }
        public Double[] doubles { get; set; }
        public Social[] social { get; set; }
        public HuntflowApplicant()
        {
            tags = new Tag[0];
            links = new Link[0];
            external = new External[0];
            doubles = new Double[0];
            social = new Social[0];
        }

        public HuntflowApplicant(string json)
        {
            tags = new Tag[0];
            links = new Link[0];
            external = new External[0];
            doubles = new Double[0];
            social = new Social[0];
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }

        public class Tag
        {
            public string v { get; set; }
            public int tag { get; set; }
            public int id { get; set; }
        }

        public class Link
        {
            public int id { get; set; }
            public int status { get; set; }
            public DateTime updated { get; set; }
            public DateTime changed { get; set; }
            public int vacancy { get; set; }
        }

        public class External
        {
            public int id { get; set; }
            public string auth_type { get; set; }
            public int account_source { get; set; }
            public DateTime updated { get; set; }
        }

        public class Agreement
        {
            public string state { get; set; }
            public DateTime decision_date { get; set; }
        }

        public class Double
        {
            public int double_var { get; set; }
        }

        public class Social
        {
            public int id { get; set; }
            public string social_type { get; set; }
            public string value { get; set; }
            public bool verified { get; set; }
            public DateTime verification_date { get; set; }
        }
    }
}