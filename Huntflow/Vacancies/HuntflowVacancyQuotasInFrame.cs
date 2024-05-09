using Newtonsoft.Json;
using System;

namespace HuntflowAPI
{
    public class HuntflowVacancyQuotasInFrame
    {
        public QuotaInfo[] Items { get; set; }

        public HuntflowVacancyQuotasInFrame()
        {
            Items = new QuotaInfo[0];
        }

        public HuntflowVacancyQuotasInFrame(string json)
        {
            Items = new QuotaInfo[0];
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }

    public class QuotaInfo
    {
        public int id { get; set; }
        public int vacancyFrame { get; set; }
        public int vacancyRequest { get; set; }
        public DateTime created { get; set; }
        public DateTime? changed { get; set; }
        public int applicantsToHire { get; set; }
        public int alreadyHired { get; set; }
        public DateTime? deadline { get; set; }
        public DateTime? closed { get; set; }
        public int workDaysInWork { get; set; }
        public int workDaysAfterDeadline { get; set; }
        public AccountInfo AccountInfo { get; set; }
    }

    public class AccountInfo
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
    }
}