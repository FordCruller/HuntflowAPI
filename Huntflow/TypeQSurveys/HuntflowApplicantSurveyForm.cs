using Newtonsoft.Json;
using System;

namespace HuntflowAPI
{
    public class HuntflowApplicantSurveyForm
    {
        public int id { get; set; }
        public string name { get; set; }
        public SurveyType type { get; set; }
        public bool active { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }
        public string schema { get; set; }
        public string ui_schema { get; set; }

        public enum SurveyType
        {
            type_a,
            type_q
        }

        public HuntflowApplicantSurveyForm() { }

        public HuntflowApplicantSurveyForm(string json)
        {
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }
}