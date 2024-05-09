using Newtonsoft.Json;
using System;

namespace HuntflowAPI
{
    public class HuntflowControlTaskResult
    {
        public Guid id { get; set; }
        public int account_id { get; set; }
        public TaskAction action { get; set; }
        public TaskStatus status { get; set; }
        public DateTime data { get; set; }
        public string comment { get; set; }
        public DateTime created { get; set; }
        public DateTime? completed { get; set; }

        public enum TaskAction
        {
            CREATE,
            UPDATE,
            DELETE
        }

        public enum TaskStatus
        {
            PENDING,
            SUCCESS,
            FAILED
        }

        public HuntflowControlTaskResult() { }

        public HuntflowControlTaskResult(string json)
        {
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }
}