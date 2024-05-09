using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HuntflowAPI
{
    public class HuntflowStatusOfASystemDelayedTask
    {
        public Guid taskId { get; set; }
        public TaskState state { get; set; }
        public long created { get; set; }
        public long? updated { get; set; }
        public DateTime createdDatetime { get; set; }
        public DateTime? updatedDatetime { get; set; }
        public List<TaskStateLog> statesLog { get; set; }
        public TaskResult result { get; set; }

        public HuntflowStatusOfASystemDelayedTask()
        {
            statesLog = new List<TaskStateLog>();
        }

        public HuntflowStatusOfASystemDelayedTask(string json)
        {
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }

    public class TaskStateLog
    {
        public TaskState state { get; set; }
        public long timestamp { get; set; }
        public DateTime datetime { get; set; }
        public string comment { get; set; }
    }

    public enum TaskState
    {
        enqueued,
        inprogress,
        success,
        failed
    }

    public abstract class TaskResult { }

    public class MultivacancyAddChildTaskResult : TaskResult
    {
        public int childVacancyId { get; set; }
    }

    public class MultivacancyUpsertTaskResult : TaskResult
    {
        public int parentVacancyId { get; set; }
        public List<int> childrenVacanciesIds { get; set; }
    }
}