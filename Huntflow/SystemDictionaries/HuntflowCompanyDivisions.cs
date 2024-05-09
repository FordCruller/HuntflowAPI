using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HuntflowAPI
{
    public class HuntflowCompanyDivisions
    {
        public List<DivisionItem> items { get; set; }
        public AllCompanyDivisionsMeta meta { get; set; }

        public HuntflowCompanyDivisions()
        {
            items = new List<DivisionItem>();
            meta = new AllCompanyDivisionsMeta();
        }

        public HuntflowCompanyDivisions(string json)
        {
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }

    public class DivisionItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public int order { get; set; }
        public bool active { get; set; }
        public int? parent { get; set; }
        public int deep { get; set; }
        public string foreign { get; set; }
        public DivisionItemMeta meta { get; set; }
    }

    public class DivisionItemMeta { }

    public class AllCompanyDivisionsMeta
    {
        public int levels { get; set; }
        public bool has_inactive { get; set; }
    }
}