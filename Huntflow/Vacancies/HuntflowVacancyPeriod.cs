using Newtonsoft.Json;
using System.Collections.Generic;

namespace HuntflowAPI
{
    public class HuntflowVacancyPeriod
    {
        public int daysInWork { get; set; }
        public int workDaysInWork { get; set; }
        public List<List<string>> holdPeriods { get; set; }
        public List<List<string>> closedPeriods { get; set; }

        public HuntflowVacancyPeriod()
        {
            holdPeriods = new List<List<string>>();
            closedPeriods = new List<List<string>>();
        }

        public HuntflowVacancyPeriod(string json)
        {
            holdPeriods = new List<List<string>>();
            closedPeriods = new List<List<string>>();
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }
}