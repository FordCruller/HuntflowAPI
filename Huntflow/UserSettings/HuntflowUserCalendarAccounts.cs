using Newtonsoft.Json;
using System.Collections.Generic;

namespace HuntflowAPI
{
    public class HuntflowUserCalendarAccountList
    {
        public List<HuntflowUserCalendarAccount> items { get; set; }

        public HuntflowUserCalendarAccountList()
        {
            items = new List<HuntflowUserCalendarAccount>();
        }

        public class HuntflowUserCalendarAccount
        {
            public int id { get; set; }
            public string name { get; set; }
            public string auth_type { get; set; }
            public bool freebusy { get; set; }
            public Calendar[] calendars { get; set; }
        }

        public class Calendar
        {
            public int id { get; set; }
            public string foreign { get; set; }
            public string name { get; set; }
            public string access_role { get; set; }
        }

        public HuntflowUserCalendarAccountList(string json)
        {
            items = new List<HuntflowUserCalendarAccount>();
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }
}