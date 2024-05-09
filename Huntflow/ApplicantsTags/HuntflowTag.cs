using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HuntflowAPI
{
    public class HuntflowTags
    {
        public HuntflowTag[] items { get; set; }

        public class HuntflowTag
        {
            public int id { get; set; }
            public string name { get; set; }
            public string color { get; set; }
            
            public HuntflowTag() { }

            public HuntflowTag(string json)
            {
                JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
            }
        }

        public HuntflowTags()
        {
            items = new HuntflowTag[0];
        }

        public HuntflowTags(string json)
        {
            items = new HuntflowTag[0];
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }

    public class TagIds
    {
        public List<int> tags { get; set; }

        public TagIds()
        {
            tags = new List<int>();
        }

        public TagIds(string json)
        {
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }
}