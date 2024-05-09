using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HuntflowAPI
{
    public class HuntflowSurveyQuestionarySchemas
    {
        public SurveySchema[] items { get; set; }

        public class SurveySchema
        {
            public int id { get; set; }
            public DateTime created { get; set; }
            public SurveyData survey { get; set; }
            public Respondent respondent { get; set; }
            public SurveyDetail survey_questionary { get; set; }
            public string data { get; set; }
            public CreatedBy created_by { get; set; }
            public int survey_answer_id { get; set; }
            public string link { get; set; }
            public string name { get; set; }
            public SurveyType type { get; set; }
            public bool active { get; set; }
            public DateTime updated { get; set; }
            public string schema { get; set; }
            public string ui_schema { get; set; }
            public string title { get; set; }

            public enum SurveyType
            {
                type_a,
                type_q
            }

            public class SurveyDetail
            {
                public int id { get; set; }
                public DateTime created { get; set; }
                public CreatedBy created_by { get; set; }
            }

            public class CreatedBy
            {
                public int account_id { get; set; }
                public string name { get; set; }
            }

            public class SurveyData
            {
                public int id { get; set; }
                public string name { get; set; }
                public SurveyType type { get; set; }
                public bool active { get; set; }
                public DateTime created { get; set; }
                public DateTime updated { get; set; }
                public SurveySchema schema { get; set; }
                public string ui_schema { get; set; }
                public string title { get; set; }
            }

            public class Respondent
            {
                public int applicant_id { get; set; }
                public string name { get; set; }
            }

            public SurveySchema() { }

            public SurveySchema(string json)
            {
                JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
            }
        }

        public HuntflowSurveyQuestionarySchemas() { }

        public HuntflowSurveyQuestionarySchemas(string json)
        {
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }
}