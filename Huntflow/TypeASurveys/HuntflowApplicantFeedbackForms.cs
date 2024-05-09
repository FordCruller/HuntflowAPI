using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HuntflowAPI
{
    public class HuntflowApplicantFeedbackForms
    {
        public List<HuntflowApplicantFeedbackForm> items { get; set; }

        public class HuntflowApplicantFeedbackForm
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

            public HuntflowApplicantFeedbackForm() { }

            public HuntflowApplicantFeedbackForm(string json)
            {
                JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
            }
        }

        public enum SurveyType
        {
            type_a,
            type_q
        }

        public HuntflowApplicantFeedbackForms() { }

        public HuntflowApplicantFeedbackForms(string json)
        {
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }
}