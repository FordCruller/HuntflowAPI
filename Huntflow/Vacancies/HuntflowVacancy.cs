using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HuntflowAPI
{
    public class HuntflowVacancyList
    {
        public int page { get; set; }
        public int count { get; set; }
        public int total_pages { get; set; }
        public int total_items { get; set; }
        public HuntflowVacancy[] items { get; set; }

        public class HuntflowVacancy
        {
            public int account_division { get; set; }
            public int account_region { get; set; }
            public string position { get; set; }
            public string company { get; set; }
            public string money { get; set; }
            public int priority { get; set; }
            public bool hidden { get; set; }
            public GetRequest.VacancyState state { get; set; }
            public int id { get; set; }
            public DateTime created { get; set; }
            public List<string> additional_fields_list { get; set; }
            public bool multiple { get; set; }
            public int parent { get; set; }
            public int account_vacancy_status_group { get; set; }

            public HuntflowVacancy() {}

            public HuntflowVacancy(string json)
            {
                JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
            }

            public override string ToString()
            {
                return $"[{id}]: {position}";
            }
        }

        public HuntflowVacancyList()
        {
            items = new HuntflowVacancy[0];
        }

        public HuntflowVacancyList(string json)
        {
            items = new HuntflowVacancy[0];
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }

        public HuntflowVacancyList(HuntflowVacancy[] vacancies)
        {
            page = 1;
            count = vacancies.Length;
            total_pages = 1;
            total_items = vacancies.Length;

            this.items = vacancies;
        }
    }
}