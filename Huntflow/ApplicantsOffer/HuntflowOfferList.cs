using Newtonsoft.Json;
using System;

namespace HuntflowAPI
{
    public class HuntflowOfferList
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool active { get; set; }
        public string template { get; set; }
        public HuntflowQuestionarySchema schema { get; set; }

        public HuntflowOfferList(string json)
        {
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }

    public class HuntflowApplicantsVacancyOffer
    {
        public int id { get; set; }
        public int accountApplicantOffer { get; set; }
        public DateTime created { get; set; }
        public QuestionaryField values { get; set; }

        public HuntflowApplicantsVacancyOffer(string json)
        {
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }
}