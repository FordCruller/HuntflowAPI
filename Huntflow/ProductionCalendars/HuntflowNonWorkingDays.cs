using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HuntflowAPI
{
    public class HuntflowNonWorkingDays
    {
        public DateTime start { get; set; }
        public DateTime deadline { get; set; }
        public int total_days { get; set; }
        public int not_working_days { get; set; }
        public int production_calendar { get; set; }
        public List<DateTime> items { get; set; }

        public HuntflowNonWorkingDays() { }

        public HuntflowNonWorkingDays(string json)
        {
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }
}