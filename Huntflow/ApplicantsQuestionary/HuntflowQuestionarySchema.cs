using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HuntflowAPI
{
    public class HuntflowQuestionarySchema
    {
        public Dictionary<string, QuestionaryField> fields { get; set; }

        public HuntflowQuestionarySchema()
        {
            fields = new Dictionary<string, QuestionaryField>();
        }

        public HuntflowQuestionarySchema(string json)
        {
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }

    public class QuestionaryField
    {
        public GetRequest.FieldType type { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public bool required { get; set; }
        public int order { get; set; }
        public object[] values { get; set; }
        public string value { get; set; }
        public Dictionary<string, QuestionaryField> fields { get; set; }
        public bool show_in_profile { get; set; }
        public string dictionary { get; set; }
    }
}