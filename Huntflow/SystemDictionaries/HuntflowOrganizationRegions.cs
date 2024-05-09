using Newtonsoft.Json;
using System.Collections.Generic;

namespace HuntflowAPI
{
    public class HuntflowOrganizationRegions
    {
        public List<RegionItem> items { get; set; }
        public OrganizationRegionsMeta meta { get; set; }

        public HuntflowOrganizationRegions()
        {
            items = new List<RegionItem>();
            meta = new OrganizationRegionsMeta();
        }

        public HuntflowOrganizationRegions(string json)
        {
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }

    public class RegionItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public int order { get; set; }
        public int? parent { get; set; }
        public int deep { get; set; }
    }

    public class OrganizationRegionsMeta
    {
        public int levels { get; set; }
        public bool has_inactive { get; set; }
    }
}