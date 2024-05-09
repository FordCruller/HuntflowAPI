using Newtonsoft.Json;

namespace HuntflowAPI
{
    public class HuntflowOrganizationsProductionCalendar
    {
        public int account { get; set; }
        public int production_calendar { get; set; }

        public HuntflowOrganizationsProductionCalendar() { }

        public HuntflowOrganizationsProductionCalendar(string json)
        {
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }
}