using System;
using static HuntflowAPI.HuntflowApplicantWorklogs;

namespace HuntflowAPI.Data.Huntflow
{
    public class Worklog
    {
        public int id { get; set; }
        public string type { get; set; }
        public int vacancy { get; set; }
        public int statusId { get; set; }
        public string source { get; set; }
        public int rejectionReasonId { get; set; }
        public DateTime created { get; set; }
        public DateTime employmentDate { get; set; }
        public AccountInfo accountInfo { get; set; }
        public string comment { get; set; }
        public File[] files { get; set; }
        public CalendarEvent calendarEvent { get; set; }
        public HiredInFillQuota hiredInFillQuota { get; set; }
        public ApplicantOffer applicantOffer { get; set; }
        public Email email { get; set; }
        public SurveyQuestionary surveyQuestionary { get; set; }

        public Worklog(HuntflowApplicantWorklog huntflowApplicantWorklog)
        {
            id = huntflowApplicantWorklog.id;
            type = huntflowApplicantWorklog.type;
            vacancy = huntflowApplicantWorklog.vacancy;
            statusId = huntflowApplicantWorklog.status;
            source = huntflowApplicantWorklog.source;
            rejectionReasonId = huntflowApplicantWorklog.rejection_reason;
            created = huntflowApplicantWorklog.created;
            employmentDate = huntflowApplicantWorklog.employment_date;
            if (huntflowApplicantWorklog.account_info != null)
            {
                accountInfo = new AccountInfo(huntflowApplicantWorklog.account_info);
            }
            comment = huntflowApplicantWorklog.comment;
            if (huntflowApplicantWorklog.files != null)
            {
                files = HuntflowData.Converter.ConvertArray<File>(huntflowApplicantWorklog.files);
            }
            if (huntflowApplicantWorklog.calendar_event != null)
            {
                calendarEvent = new CalendarEvent(huntflowApplicantWorklog.calendar_event);
            }
            if (huntflowApplicantWorklog.hired_in_fill_quota != null)
            {
                hiredInFillQuota = new HiredInFillQuota(huntflowApplicantWorklog.hired_in_fill_quota);
            }
            if (huntflowApplicantWorklog.applicant_offer != null)
            {
                applicantOffer = new ApplicantOffer(huntflowApplicantWorklog.applicant_offer);
            }
            if (huntflowApplicantWorklog.email != null)
            {
                email = new Email(huntflowApplicantWorklog.email);
            }
            if (huntflowApplicantWorklog.survey_questionary != null)
            {
                surveyQuestionary = new SurveyQuestionary(huntflowApplicantWorklog.survey_questionary);
            }
        }

        public Worklog()
        {
        }

        public static Worklog[] Convert(HuntflowApplicantWorklogs huntflowApplicantWorklogs)
        {
            var worklogArray = new Worklog[huntflowApplicantWorklogs.items.Length];
            if (huntflowApplicantWorklogs.items != null)
            {
                worklogArray = HuntflowData.Converter.ConvertArray<Worklog>(huntflowApplicantWorklogs.items);
            }
            return worklogArray;
        }

        public class AccountInfo
        {
            public int id { get; set; }
            public string name { get; set; }
            public string email { get; set; }

            public AccountInfo(HuntflowApplicantWorklog.AccountInfo accountInfo)
            {
                if (accountInfo != null)
                {
                    id = accountInfo.id;
                    name = accountInfo.name;
                    email = accountInfo.email;
                }
            }
        }

        public class File
        {
            public int id { get; set; }
            public Uri url { get; set; }
            public string contentType { get; set; }
            public string name { get; set; }

            public File(HuntflowApplicantWorklog.File file)
            {
                if (file != null)
                {
                    id = file.id;
                    url = file.url;
                    contentType = file.content_type;
                    name = file.name;
                }
            }
        }

        public class CalendarEvent
        {
            public int id { get; set; }
            public string name { get; set; }
            public bool allDay { get; set; }
            public DateTime created { get; set; }
            public Creator creator { get; set; }
            public string description { get; set; }
            public string timezone { get; set; }
            public DateTime start { get; set; }
            public DateTime end { get; set; }
            public string etag { get; set; }
            public string eventType { get; set; }
            public int interviewType { get; set; }
            public int calendar { get; set; }
            public int vacancy { get; set; }
            public string foreign { get; set; }
            public string location { get; set; }
            public Attendee[] attendees { get; set; }
            public Reminder[] reminders { get; set; }
            public string status { get; set; }
            public string transparency { get; set; }
            public string[] recurrence { get; set; }

            public CalendarEvent() 
            {
                attendees = new Attendee[0];
                reminders = new Reminder[0];
                recurrence = new string[0];
            }

            public CalendarEvent(HuntflowApplicantWorklog.CalendarEvent calendarEvent)
            {
                id = calendarEvent.id;
                name = calendarEvent.name;
                allDay = calendarEvent.all_day;
                created = calendarEvent.created;
                if(calendarEvent.creator != null)
                {
                    creator = new Creator(calendarEvent.creator);
                }
                description = calendarEvent.description;
                timezone = calendarEvent.timezone;
                start = calendarEvent.start;
                end = calendarEvent.end;
                etag = calendarEvent.etag;
                eventType = calendarEvent.event_type;
                interviewType = calendarEvent.interview_type;
                calendar = calendarEvent.calendar;
                vacancy = calendarEvent.vacancy;
                foreign = calendarEvent.foreign;
                location = calendarEvent.location;
                if (calendarEvent.attendees != null)
                {
                    attendees = HuntflowData.Converter.ConvertArray<Attendee>(calendarEvent.attendees);
                }
                if (calendarEvent.reminders != null)
                {
                    reminders = HuntflowData.Converter.ConvertArray<Reminder>(calendarEvent.reminders);
                }
                status = calendarEvent.status;
                transparency = calendarEvent.transparency;
                recurrence = calendarEvent.recurrence;
            }
        }

        public class Creator
        {
            public string displayName { get; set; }
            public string email { get; set; }
            public bool self { get; set; }

            public Creator(HuntflowApplicantWorklog.Creator creator)
            {
                displayName = creator.displayName;
                email = creator.email;
                self = creator.self;
            }

            public Creator() { }
        }

        public class Attendee
        {
            public int member { get; set; }
            public string displayName { get; set; }
            public string email { get; set; }
            public string responseStatus { get; set; }

            public Attendee() { }

            public Attendee(HuntflowApplicantWorklog.Attendee attendee)
            {
                member = attendee.member;
                displayName = attendee.displayName;
                email = attendee.email;
                responseStatus = attendee.responseStatus;
            }
        }

        public class Reminder
        {
            public string method { get; set; }
            public int minutes { get; set; }

            public Reminder() { }

            public Reminder(HuntflowApplicantWorklog.Reminder reminder)
            {
                method = reminder.method;
                minutes = reminder.minutes;
            }
        }

        public class HiredInFillQuota
        {
            public int id { get; set; }
            public int vacancyFrame { get; set; }
            public int vacancyRequest { get; set; }
            public DateTime created { get; set; }
            public DateTime changed { get; set; }
            public int applicantsToHire { get; set; }
            public int alreadyHired { get; set; }
            public DateTime deadline { get; set; }
            public DateTime closed { get; set; }
            public int workDaysInWork { get; set; }
            public int workDaysAfterDeadline { get; set; }
            public AccountInfo accountInfo { get; set; }

            public HiredInFillQuota() { }

            public HiredInFillQuota(HuntflowApplicantWorklog.HiredInFillQuota hiredInFillQuota)
            {
                id = hiredInFillQuota.id;
                vacancyFrame = hiredInFillQuota.vacancy_frame;
                vacancyRequest = hiredInFillQuota.vacancy_request;
                created = hiredInFillQuota.created;
                changed = hiredInFillQuota.changed;
                applicantsToHire = hiredInFillQuota.applicants_to_hire;
                alreadyHired = hiredInFillQuota.already_hired;
                deadline = hiredInFillQuota.deadline;
                closed = hiredInFillQuota.closed;
                workDaysInWork = hiredInFillQuota.work_days_in_work;
                workDaysAfterDeadline = hiredInFillQuota.work_days_after_deadline;

                if(hiredInFillQuota.account_info != null)
                {
                    accountInfo = new AccountInfo(hiredInFillQuota.account_info);
                }
            }
        }

        public class ApplicantOffer
        {
            public int id { get; set; }
            public int accountApplicantOffer { get; set; }
            public DateTime created { get; set; }

            public ApplicantOffer() { }

            public ApplicantOffer(HuntflowApplicantWorklog.ApplicantOffer applicantOffer)
            {
                id = applicantOffer.id;
                accountApplicantOffer = applicantOffer.account_applicant_offer;
                created = applicantOffer.created;
            }
        }

        public class Email
        {
            public int id { get; set; }
            public DateTime created { get; set; }
            public string subject { get; set; }
            public int emailThread { get; set; }
            public int accountEmail { get; set; }
            public File[] files { get; set; }
            public string foreign { get; set; }
            public string timezone { get; set; }
            public string html { get; set; }
            public string fromEmail { get; set; }
            public string fromName { get; set; }
            public string[] replyto { get; set; }
            public DateTime sendAt { get; set; }
            public Recipient[] to { get; set; }
            public string state { get; set; }

            public Email() { }

            public Email(HuntflowApplicantWorklog.Email email)
            {
                id = email.id;
                created = email.created;
                subject = email.subject;
                emailThread = email.email_thread;
                accountEmail = email.account_email;
                if (email.files != null)
                {
                    files = HuntflowData.Converter.ConvertArray<File>(email.files);
                }
                foreign = email.foreign;
                timezone = email.timezone;
                html = email.html;
                fromEmail = email.from_email;
                fromName = email.from_name;
                replyto = email.replyto;
                sendAt = email.send_at;
                if (email.to != null)
                {
                    to = HuntflowData.Converter.ConvertArray<Recipient>(email.to);
                }
                state = email.state;
            }
        }

        public class Recipient
        {
            public string type { get; set; }
            public string displayName { get; set; }
            public string email { get; set; }

            public Recipient() { }

            public Recipient(HuntflowApplicantWorklog.Recipient recipient)
            {
                type = recipient.type;
                displayName = recipient.displayName;
                email = recipient.email;
            }
        }

        public class SurveyQuestionary
        {
            public int id { get; set; }
            public Survey survey { get; set; }
            public int surveyAnswerId { get; set; }
            public DateTime created { get; set; }

            public SurveyQuestionary() { }

            public SurveyQuestionary(HuntflowApplicantWorklog.SurveyQuestionary surveyQuestionary)
            {
                id = surveyQuestionary.id;
                survey = new Survey(surveyQuestionary.survey);
                surveyAnswerId = surveyQuestionary.survey_answer_id;
                created = surveyQuestionary.created;
            }
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

            public Survey() { }

            public Survey(HuntflowApplicantWorklog.Survey survey)
            {
                id = survey.id;
                name = survey.name;
                type = survey.type;
                active = survey.active;
                created = survey.created;
                updated = survey.updated;
                title = survey.title;
            }
        }

        public class SurveyAnswer
        {
            public int surveyAnswerId { get; set; }

            public SurveyAnswer() { }

            public SurveyAnswer(HuntflowApplicantWorklog.SurveyAnswer surveyAnswer)
            {
                surveyAnswerId = surveyAnswer.survey_answer_id;
            }
        }
    }

    public class ApplicantWorklogs
    {
        public int applicantId { get; set; }
        public Worklog[] logs { get; set; }

        public ApplicantWorklogs() { }

        public ApplicantWorklogs(int applicantId, Worklog[] worklogs)
        {
            this.applicantId = applicantId;
            this.logs = worklogs;
        }
    }
}
