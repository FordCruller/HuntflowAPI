using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using static HuntflowAPI.HuntflowVacancyRequestSchemaList;

namespace HuntflowAPI
{
    public class HuntflowVacancyRequestSchemaList
    {
        public List<HuntflowVacancyRequestSchema> items { get; set; }

        public HuntflowVacancyRequestSchemaList()
        {
            items = new List<HuntflowVacancyRequestSchema>();
        }

        public class ApprovalState
        {
            public int id { get; set; }
            public string status { get; set; }
            public string email { get; set; }
            public string reason { get; set; }
            public int order { get; set; }
            public DateTime changed { get; set; }
        }

        public class HuntflowVacancyRequestSchema
        {
            public int id { get; set; }
            public int account { get; set; }
            public string name { get; set; }
            public string attendee_required { get; set; }
            public string attendee_hint { get; set; }
            public bool active { get; set; }
            public Dictionary<string, SchemaField> schema { get; set; }

            public HuntflowVacancyRequestSchema()
            {
            }

            public HuntflowVacancyRequestSchema(string json)
            {
                JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
            }
        }

        public class SchemaField
        {
            public int id { get; set; }
            public string type { get; set; }
            public string title { get; set; }
            public bool required { get; set; }
            public int order { get; set; }
            public List<object> values { get; set; }
            public string value { get; set; }
            public Dictionary<string, SchemaField> fields { get; set; }
            public SchemaField(string json)
            {
                JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
            }
        }

        public HuntflowVacancyRequestSchemaList(string json)
        {
            items = new List<HuntflowVacancyRequestSchema>();
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }

    public class VacancyRequestList
    {
        public int page { get; set; }
        public int count { get; set; }
        public int total_pages { get; set; }
        public int total_items { get; set; }
        public List<HuntflowVacancyRequest> items { get; set; }

        public VacancyRequestList()
        {
            items = new List<HuntflowVacancyRequest>();
        }

        public class HuntflowVacancyRequest
        {
            public int id { get; set; }
            public string position { get; set; }
            public string status { get; set; }
            public int account_vacancy_request { get; set; }
            public DateTime created { get; set; }
            public DateTime? updated { get; set; }
            public DateTime changed { get; set; }
            public int vacancy { get; set; }
            public Creator creator { get; set; }
            public List<File> files { get; set; }
            public List<State> states { get; set; }
            public Dictionary<string, object> values { get; set; }

            public HuntflowVacancyRequest()
            {
            }

            public HuntflowVacancyRequest(string json)
            {
                JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
            }
        }

        public class Creator
        {
            public int id { get; set; }
            public string name { get; set; }
            public string email { get; set; }
        }

        public class File
        {
            public int id { get; set; }
            public string url { get; set; }
            public string content_type { get; set; }
            public string name { get; set; }
        }

        public class State
        {
            public int id { get; set; }
            public string status { get; set; }
            public string email { get; set; }
            public string reason { get; set; }
            public int order { get; set; }
            public DateTime changed { get; set; }
        }

        public VacancyRequestList(string json)
        {
            items = new List<HuntflowVacancyRequest>();
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }
}