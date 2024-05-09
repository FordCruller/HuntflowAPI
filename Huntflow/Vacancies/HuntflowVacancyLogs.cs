using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HuntflowAPI
{
    public class HuntflowVacancyLogs
    {
        public int page { get; set; }
        public int count { get; set; }
        public int total_pages { get; set; }
        public int total_items { get; set; }
        public List<HuntflowVacancyLog> items { get; set; }

        public HuntflowVacancyLogs()
        {
            items = new List<HuntflowVacancyLog>();
        }

        public class HuntflowVacancyLog
        {
            public int accountDivision { get; set; }
            public int accountRegion { get; set; }
            public string position { get; set; }
            public string company { get; set; }
            public string money { get; set; }
            public int priority { get; set; }
            public bool hidden { get; set; }
            public string state { get; set; }
            public int id { get; set; }
            public DateTime created { get; set; }
            public List<string> additionalFieldsList { get; set; }
            public bool multiple { get; set; }
            public int parent { get; set; }
            public int accountVacancyStatusGroup { get; set; }
        }

        public HuntflowVacancyLogs(string json)
        {
            items = new List<HuntflowVacancyLog>();
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }
}