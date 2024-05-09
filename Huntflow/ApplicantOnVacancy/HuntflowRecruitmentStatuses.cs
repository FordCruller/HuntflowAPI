using Newtonsoft.Json;
using System;

namespace HuntflowAPI
{
    public class HuntflowRecruitmentStatuses
    {
        public Status[] items { get; set; }

        public class Status
        {
            public int id { get; set; }
            public string type { get; set; }
            public string name { get; set; }
            public DateTime? removed { get; set; }
            public int order { get; set; }
            public int? stay_duration { get; set; }
        }

        public HuntflowRecruitmentStatuses()
        {
            items = new Status[0];
        }

        public HuntflowRecruitmentStatuses(string json)
        {
            items = new Status[0];
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }

    public class RecruitmentStatusGroups
    {
        public Group[] items { get; set; }

        public class Group
        {
            public int id { get; set; }
            public string name { get; set; }
            public Status[] statuses { get; set; }
        }

        public class Status
        {
            public int id { get; set; }
            public int accountVacancyStatus { get; set; }
            public int? stayDuration { get; set; }
        }

        public RecruitmentStatusGroups()
        {
            items = new Group[0];
        }

        public RecruitmentStatusGroups(string json)
        {
            items = new Group[0];
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }
}