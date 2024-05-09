using Newtonsoft.Json;

namespace HuntflowAPI
{
    public class HuntflowResumeSources
    {
        public Source[] items { get; set; }

        public class Source
        {
            public int id { get; set; }
            public string foreign { get; set; }
            public string name { get; set; }
            public string type { get; set; }
        }

        public HuntflowResumeSources()
        {
            items = new Source[0];
        }

        public HuntflowResumeSources(string json)
        {
            items = new Source[0];
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }
}