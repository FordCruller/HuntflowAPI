using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HuntflowAPI.Data.Huntflow
{
    public class ManualSettings
    {
        public HotSources hot_sources { get; set; }
        public int reject_id { get; set; }
        public int[] step_by_step { get; set; }

        public Interview[] interview_types { get; set; }

        public int final_step_id { get; set; }

        public int[] applicant_reject_reasons { get; set; }

        public int[] company_reject_reasons { get; set; }

        public int referal_tag_id { get; set; }

        public int offer_received_step_id { get; set; }

        public int offer_accepted_step_id { get; set; }

        public int hot_tag_id { get; set; }

        private static ManualSettings instance;

        private ManualSettings(string json)
        {
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }

        public static ManualSettings GetInstance()
        {
            if (instance == null)
            {
                string executingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string filePath = Path.GetFullPath(Path.Combine(executingPath, @"..\..\Data\Huntflow\settings.json"));
                string json = System.IO.File.ReadAllText(filePath);
                instance = new ManualSettings(json);
            }
            return instance;
        }

        public Dictionary<int, Interview.Type> GetInterviewTypesDictionary()
        {
            var result = new Dictionary<int, Interview.Type>();
            foreach (var type in interview_types)
            {
                result.Add(type.id, type.type);
            }
            return result;
        }

        public class Interview
        {
            public Type type { get; set; }
            public int id { get; set; }

            public enum Type
            {
                SIMPLE,
                PHONE,
                SKYPE,
                GROUP,
                ONLINE,
                OTHER
            }
        }

        public enum RejectReasonType
        {
            APPLICANT,
            COMPANY,
            OTHER
        }
    }

    public class HotSources
    {
        public int hh_id { get; set; }
    }
}
