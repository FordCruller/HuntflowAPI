using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using static HuntflowAPI.GetRequest;

namespace HuntflowAPI
{
    public class HuntflowSecurityLogs
    {
        public List<LogItem> items { get; set; }
        public int nextId { get; set; }

        public HuntflowSecurityLogs()
        {
            items = new List<LogItem>();
        }

        public HuntflowSecurityLogs(string json)
        {
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }

    public class LogItem
    {
        public int id { get; set; }
        public HuntflowUsers.HuntflowUser user { get; set; }
        public LogType logType { get; set; }
        public DateTime created { get; set; }
        public string action { get; set; }
        public string ipv4 { get; set; }
        public string data { get; set; }
    }
}