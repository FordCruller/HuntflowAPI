using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HuntflowAPI.HuntflowVacancyList;

namespace HuntflowAPI.Data
{
    public class Vacancy
    {
        public int accountDivision { get; set; }
        public int accountRegion { get; set; }
        public string position { get; set; }
        public string company { get; set; }
        public string money { get; set; }
        public int priority { get; set; }
        public bool hidden { get; set; }
        public GetRequest.VacancyState state { get; set; }
        public int id { get; set; }
        public DateTime created { get; set; }
        public List<string> additionalFieldsList { get; set; }
        public bool multiple { get; set; }
        public int parent { get; set; }
        public int accountVacancyStatusGroup { get; set; }

        internal Vacancy(HuntflowVacancy vacancyFromHuntflow) 
        {
            accountDivision = vacancyFromHuntflow.account_division;
            accountRegion = vacancyFromHuntflow.account_region;
            position = vacancyFromHuntflow.position;
            company = vacancyFromHuntflow.company;
            money = vacancyFromHuntflow.money;
            priority = vacancyFromHuntflow.priority;
            hidden = vacancyFromHuntflow.hidden;
            state = vacancyFromHuntflow.state;
            id = vacancyFromHuntflow.id;
            created = vacancyFromHuntflow.created;
            additionalFieldsList = vacancyFromHuntflow.additional_fields_list;
            multiple = vacancyFromHuntflow.multiple;
            parent = vacancyFromHuntflow.parent;
            accountVacancyStatusGroup = vacancyFromHuntflow.account_vacancy_status_group;
        }

        public static Vacancy[] Convert(HuntflowVacancyList vacanciesFromHuntflow)
        {
            var vacancies = new Vacancy[vacanciesFromHuntflow.items.Length];
            for (int i = 0; i < vacancies.Length; i++)
            {
                vacancies[i] = new Vacancy(vacanciesFromHuntflow.items[i]);
            }
            return vacancies;
        }
    }
}
