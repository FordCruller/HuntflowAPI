using Newtonsoft.Json;
using System;

namespace HuntflowAPI
{
    public class HuntflowEmailTemplates
    {
        public Item[] items { get; set; }

        public class Item
        {
            public int id { get; set; }
            public string subject { get; set; }
            public string name { get; set; }
            public int member { get; set; }
            public string html { get; set; }
            public string type { get; set; }
            public Followup[] followups { get; set; }
            public Attendee[] attendees { get; set; }
            public Division[] divisions { get; set; }
            public File[] files { get; set; }
        }

        public class Followup
        {
            public int id { get; set; }
            public int account_member_template { get; set; }
            public string html { get; set; }
            public int days { get; set; }
        }

        public class Attendee
        {
            public string type { get; set; }
            public string email { get; set; }
        }

        public class Division
        {
            public int id { get; set; }
        }

        public class File
        {
            public int id { get; set; }
            public Uri url { get; set; }
            public string content_type { get; set; }
            public string name { get; set; }
        }

        public HuntflowEmailTemplates() { }

        public HuntflowEmailTemplates(string json)
        {
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }
}