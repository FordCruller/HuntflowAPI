using Newtonsoft.Json;
using System.Collections.Generic;

namespace HuntflowAPI
{
    public class HuntflowOrganizations
    {
        public HuntflowOrganization[] items { get; set; }

        public HuntflowOrganizations(string json)
        {
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }

        public HuntflowOrganizations() { }
    }

    public class HuntflowOrganization
    {
        public int id { get; set; }
        public string name { get; set; }
        public string nick { get; set; }
        public string member_type { get; set; }
        public int production_calendar { get; set; }
    }
}
