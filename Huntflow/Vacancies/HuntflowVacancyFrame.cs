using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HuntflowAPI
{
    public class HuntflowVacancyFrameList
    {
        public List<HuntflowVacancyFrame> items { get; set; }

        public HuntflowVacancyFrameList()
        {
            items = new List<HuntflowVacancyFrame>();
        }

        public HuntflowVacancyFrameList(string json)
        {
            items = new List<HuntflowVacancyFrame>();
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }

    public class HuntflowVacancyFrame
    {
        public int id { get; set; }
        public DateTime frameBegin { get; set; }
        public DateTime? frameEnd { get; set; }
        public int vacancy { get; set; }
        public List<int> hiredApplicants { get; set; }
        public int workdaysInWork { get; set; }
        public int workdaysBeforeDeadline { get; set; }
        public int nextId { get; set; }

        public HuntflowVacancyFrame(string json)
        {
            hiredApplicants = new List<int>();
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }

        public HuntflowVacancyFrame()
        {
            hiredApplicants = new List<int>();
        }
    }
}