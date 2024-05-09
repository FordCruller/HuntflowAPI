using Newtonsoft.Json;

namespace HuntflowAPI
{
    public class HuntflowVacancyReasons
    {
        public Item[] items { get; set; }

        public class Item
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        public HuntflowVacancyReasons() { }

        public HuntflowVacancyReasons(string json)
        {
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }
}