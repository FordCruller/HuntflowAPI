using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HuntflowAPI
{
    public class HuntflowCustomDictionaries
    {
        public List<CustomDictionary> items { get; set; }

        public HuntflowCustomDictionaries()
        {
            items = new List<CustomDictionary>();
        }

        public HuntflowCustomDictionaries(string json)
        {
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }

    public class CustomDictionary
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string foreign { get; set; }
        public DateTime created { get; set; }
        public List<DictionaryField> fields { get; set; }

        public CustomDictionary()
        {
            fields = new List<DictionaryField>();
        }

        public CustomDictionary(string json)
        {
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }

    public class DictionaryField
    {
        public int id { get; set; }
        public string name { get; set; }
        public int order { get; set; }
        public bool active { get; set; }
        public int? parent { get; set; }
        public int deep { get; set; }
        public string foreign { get; set; }
        public string meta { get; set; }
    }
}