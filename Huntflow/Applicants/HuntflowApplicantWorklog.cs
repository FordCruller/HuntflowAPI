using Newtonsoft.Json;
using System;

namespace HuntflowAPI
{
    public class HuntflowApplicantWorklogs
    {
        public int page { get; set; }
        public int count { get; set; }
        public int total_pages { get; set; }
        public int total_items { get; set; }
        public HuntflowApplicantWorklog[] items { get; set; }

        public HuntflowApplicantWorklogs(HuntflowApplicantWorklog[] huntflowApplicantWorklogs)
        {
            page = 1;
            count = huntflowApplicantWorklogs.Length;
            total_pages = 1;
            total_items = huntflowApplicantWorklogs.Length;

            items = huntflowApplicantWorklogs;
        }

        public class HuntflowApplicantWorklog
        {
            public int id { get; set; }
            public string type { get; set; }
            public int vacancy { get; set; }
            public int status { get; set; }
            public string source { get; set; }
            public int rejection_reason { get; set; }
            public DateTime created { get; set; }
            public DateTime employment_date { get; set; }
            public AccountInfo account_info { get; set; }
            public string comment { get; set; }
            public File[] files { get; set; }
            public CalendarEvent calendar_event { get; set; }
            public HiredInFillQuota hired_in_fill_quota { get; set; }
            public ApplicantOffer applicant_offer { get; set; }
            public Email email { get; set; }
            public SurveyQuestionary survey_questionary { get; set; }

            public class AccountInfo
            {
                public int id { get; set; }
                public string name { get; set; }
                public string email { get; set; }
            }

            public class File
            {
                public int id { get; set; }
                public Uri url { get; set; }
                public string content_type { get; set; }
                public string name { get; set; }
            }

            public class CalendarEvent
            {
                public int id { get; set; }
                public string name { get; set; }
                public bool all_day { get; set; }
                public DateTime created { get; set; }
                public Creator creator { get; set; }
                public string description { get; set; }
                public string timezone { get; set; }
                public DateTime start { get; set; }
                public DateTime end { get; set; }
                public string etag { get; set; }
                public string event_type { get; set; }
                public int interview_type { get; set; }
                public int calendar { get; set; }
                public int vacancy { get; set; }
                public string foreign { get; set; }
                public string location { get; set; }
                public Attendee[] attendees { get; set; }
                public Reminder[] reminders { get; set; }
                public string status { get; set; }
                public string transparency { get; set; }
                public string[] recurrence { get; set; }
            }

            public class Creator
            {
                public string displayName { get; set; }
                public string email { get; set; }
                public bool self { get; set; }
            }

            public class Attendee
            {
                public int member { get; set; }
                public string displayName { get; set; }
                public string email { get; set; }
                public string responseStatus { get; set; }
            }

            public class Reminder
            {
                public string method { get; set; }
                public int minutes { get; set; }
            }

            public class HiredInFillQuota
            {
                public int id { get; set; }
                public int vacancy_frame { get; set; }
                public int vacancy_request { get; set; }
                public DateTime created { get; set; }
                public DateTime changed { get; set; }
                public int applicants_to_hire { get; set; }
                public int already_hired { get; set; }
                public DateTime deadline { get; set; }
                public DateTime closed { get; set; }
                public int work_days_in_work { get; set; }
                public int work_days_after_deadline { get; set; }
                public AccountInfo account_info { get; set; }
            }

            public class ApplicantOffer
            {
                public int id { get; set; }
                public int account_applicant_offer { get; set; }
                public DateTime created { get; set; }
            }

            public class Email
            {
                public int id { get; set; }
                public DateTime created { get; set; }
                public string subject { get; set; }
                public int email_thread { get; set; }
                public int account_email { get; set; }
                public File[] files { get; set; }
                public string foreign { get; set; }
                public string timezone { get; set; }
                public string html { get; set; }
                public string from_email { get; set; }
                public string from_name { get; set; }
                public string[] replyto { get; set; }
                public DateTime send_at { get; set; }
                public Recipient[] to { get; set; }
                public string state { get; set; }
            }

            public class Recipient
            {
                public string type { get; set; }
                public string displayName { get; set; }
                public string email { get; set; }
            }

            public class SurveyQuestionary
            {
                public int id { get; set; }
                public Survey survey { get; set; }
                public int survey_answer_id { get; set; }
                public DateTime created { get; set; }
            }

            public class Survey
            {
                public int id { get; set; }
                public string name { get; set; }
                public string type { get; set; }
                public bool active { get; set; }
                public DateTime created { get; set; }
                public DateTime updated { get; set; }
                public string title { get; set; }
            }

            public class SurveyAnswer
            {
                public int survey_answer_id { get; set; }
            }
        }

        public HuntflowApplicantWorklogs()
        {
            items = new HuntflowApplicantWorklog[0];
        }

        public HuntflowApplicantWorklogs(string json)
        {
            items = new HuntflowApplicantWorklog[0];
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }
}