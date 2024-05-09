using Newtonsoft.Json;
using System.Collections.Generic;

namespace HuntflowAPI
{
    public class HuntflowProductionCalendars
    {
        public List<ProductionCalendar> items { get; set; }

        public HuntflowProductionCalendars()
        {
            items = new List<ProductionCalendar>();
        }

        public HuntflowProductionCalendars(string json)
        {
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }

    public class ProductionCalendar
    {
        public int id { get; set; }
        public string name { get; set; }

        public ProductionCalendar() { }

        public ProductionCalendar(string json)
        {
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }
}