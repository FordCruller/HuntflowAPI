using HuntflowAPI.Data.Huntflow;
using System;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HuntflowAPI.Order
{
    internal class DashboardReport
    {
        public int applicantsCount;

        public int hotApplicantsCount;
        public int coldApplicantsCount;
        public int referalApplicantCount;

        public InterviewCount phoneInterviewCounts;
        public InterviewCount hrInterviewCounts;
        public InterviewCount finalInterviewCounts;

        public OldInterviewCount[] oldInterviewCounts;

        public RejectionReasonCount[] companyRejectReasonCounts;
        public RejectionReasonCount[] applicantRejectReasonCounts;

        public int otherRejectReasonCount;

        public int offerCount;
        public int companyRejectOfOfferCount;
        public int applicantRejectOfOfferCount;

        public string vacancyName;

        internal static ManualSettings settings = ManualSettings.GetInstance();

        public DashboardReport(ApplicantsOrderData data)
        {
            vacancyName = data.vacancy.position;

            applicantsCount = data.applicants.Length;

            hotApplicantsCount = 0;
            coldApplicantsCount = 0;
            referalApplicantCount = 0;

            for (int i = 0; i < data.applicants.Length; i++)
            {
                if (data.applicants[i].IsReferal())
                {
                    referalApplicantCount++;
                }
                else if (data.applicants[i].IsHot(data.worklogs[i].logs))
                {
                    hotApplicantsCount++;
                }
                else
                {
                    coldApplicantsCount++;
                }
            }

            offerCount = 0;
            companyRejectOfOfferCount = 0;
            applicantRejectOfOfferCount = 0;

            foreach (var applicant in data.worklogs)
            {
                bool foundReject = false;
                bool foundOfferStep = false;
                ManualSettings.RejectReasonType reasonType = ManualSettings.RejectReasonType.COMPANY;
                foreach (var log in applicant.logs)
                {
                    if (log.statusId == settings.reject_id)
                    {
                        foundReject = true;
                        reasonType = settings.applicant_reject_reasons.Contains(log.rejectionReasonId) ? ManualSettings.RejectReasonType.APPLICANT : ManualSettings.RejectReasonType.COMPANY;
                    }
                    if (log.statusId == settings.offer_received_step_id)
                    {
                        foundOfferStep = true;
                        offerCount++;
                    }
                }
                if (foundReject && foundOfferStep)
                {
                    if (reasonType == ManualSettings.RejectReasonType.APPLICANT)
                    {
                        applicantRejectOfOfferCount++;
                    }
                    else if (reasonType == ManualSettings.RejectReasonType.COMPANY)
                    {
                        companyRejectOfOfferCount++;
                    }
                }
            }

            int hotPhoneInterviewCount = 0;
            int coldPhoneInterviewCount = 0;
            int hotHrInterviewCount = 0;
            int coldHrInterviewCount = 0;
            int hotFinalInterviewCount = 0;
            int coldFinalInterviewCount = 0;

            for (int a = 0; a < data.applicants.Length; a++)
            {
                var logs = data.worklogs[a].logs;
                bool isHot = data.applicants[a].IsHot(logs);
                if (ContainsInterview(logs, InterviewCount.Type.PHONE))
                {
                    if (isHot)
                    {
                        hotPhoneInterviewCount++;
                    }
                    else
                    {
                        coldPhoneInterviewCount++;
                    }
                }
                if (ContainsInterview(logs, InterviewCount.Type.HR))
                {
                    if (isHot)
                    {
                        hotHrInterviewCount++;
                    }
                    else
                    {
                        coldHrInterviewCount++;
                    }
                }
                if (ContainsInterview(logs, InterviewCount.Type.FINAL))
                {
                    if (isHot)
                    {
                        hotFinalInterviewCount++;
                    }
                    else
                    {
                        coldFinalInterviewCount++;
                    }
                }
            }

            phoneInterviewCounts = new InterviewCount(coldPhoneInterviewCount, hotPhoneInterviewCount, InterviewCount.Type.PHONE);
            hrInterviewCounts = new InterviewCount(coldHrInterviewCount, hotHrInterviewCount, InterviewCount.Type.HR);
            finalInterviewCounts = new InterviewCount(coldFinalInterviewCount, hotFinalInterviewCount, InterviewCount.Type.FINAL);

            var rejectReasonCounts = new RejectionReasonCount[data.rejectionReasons.Length];
            for (int i = 0; i < rejectReasonCounts.Length; i++)
            {
                rejectReasonCounts[i] = new RejectionReasonCount(data.worklogs, data.rejectionReasons[i]);
            }

            var companyRejectReasonCountsList = new List<RejectionReasonCount>();
            var applicantRejectReasonCountsList = new List<RejectionReasonCount>();

            otherRejectReasonCount = 0;
            foreach (var count in rejectReasonCounts)
            {
                if (count.type == ManualSettings.RejectReasonType.APPLICANT)
                {
                    applicantRejectReasonCountsList.Add(count);
                }
                else if (count.type == ManualSettings.RejectReasonType.COMPANY)
                {
                    companyRejectReasonCountsList.Add(count);
                }
                else
                {
                    otherRejectReasonCount += count.count;
                }
            }

            var companyRejectReasonCounts = companyRejectReasonCountsList.ToArray();
            Array.Sort(companyRejectReasonCounts, (p1, p2) => p2.count.CompareTo(p1.count));
            var applicantRejectReasonCounts = applicantRejectReasonCountsList.ToArray();
            Array.Sort(applicantRejectReasonCounts, (p1, p2) => p2.count.CompareTo(p1.count));

            this.companyRejectReasonCounts = companyRejectReasonCounts;
            this.applicantRejectReasonCounts = applicantRejectReasonCounts;

            oldInterviewCounts = OldInterviewCount.GetCountsFromLogs(data.worklogs, data.applicants);
        }

        private static bool ContainsInterview(Worklog[] logs, InterviewCount.Type type)
        {
            bool passedFinalStep = false;
            foreach (var log in logs)
            {
                if (log.statusId != -1 && settings.step_by_step.Contains(log.statusId) && Array.IndexOf(settings.step_by_step, log.statusId) >= Array.IndexOf(settings.step_by_step, settings.final_step_id))
                {
                    passedFinalStep = true;
                    break;
                }
            }

            foreach (var log in logs)
            {
                if (log.calendarEvent != null && !log.calendarEvent.status.Equals("cancelled"))
                {
                    ManualSettings.Interview.Type interviewType = ManualSettings.Interview.Type.OTHER;
                    try
                    {
                        interviewType = settings.GetInterviewTypesDictionary()[log.calendarEvent.interviewType];
                    } catch (KeyNotFoundException ex) { Console.WriteLine(ex.Message); }

                    if (type == InterviewCount.Type.PHONE && interviewType == ManualSettings.Interview.Type.PHONE
                        || type == InterviewCount.Type.HR && !passedFinalStep && interviewType != ManualSettings.Interview.Type.PHONE
                        || type == InterviewCount.Type.FINAL && passedFinalStep && interviewType != ManualSettings.Interview.Type.PHONE)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        internal class RejectionReasonCount
        {
            internal string name;
            internal int count;
            internal ManualSettings.RejectReasonType type;

            public RejectionReasonCount(ApplicantWorklogs[] applicantWorklogs, RejectionReason rejectionReason)
            {
                count = 0;
                name = rejectionReason.name;
                foreach (var applicant in applicantWorklogs) 
                {
                    foreach (var log in applicant.logs)
                    {
                        if (log.statusId == settings.reject_id && rejectionReason.id == log.rejectionReasonId)
                        {
                            count++;
                        }
                    }
                }

                if (settings.company_reject_reasons.Contains(rejectionReason.id))
                {
                    type = ManualSettings.RejectReasonType.COMPANY;
                }
                else if (settings.applicant_reject_reasons.Contains(rejectionReason.id))
                {
                    type = ManualSettings.RejectReasonType.APPLICANT;
                }
                else
                {
                    type = ManualSettings.RejectReasonType.OTHER;
                }
            }
        }

        internal class OldInterviewCount
        {
            public int cold;
            public int hot;
            public ManualSettings.Interview.Type type;
            public string name;

            internal OldInterviewCount(int cold, int hot, ManualSettings.Interview.Type type)
            {
                this.cold = cold;
                this.hot = hot;
                this.type = type;

                if (type == ManualSettings.Interview.Type.SIMPLE)
                {
                    name = "Интервью";
                }
                else if (type == ManualSettings.Interview.Type.PHONE)
                {
                    name = "Телефонное интервью";
                }
                else if (type == ManualSettings.Interview.Type.SKYPE)
                {
                    name = "Скайп-интервью";
                }
                else if (type == ManualSettings.Interview.Type.GROUP)
                {
                    name = "Групповое интервью";
                }
                else if (type == ManualSettings.Interview.Type.ONLINE)
                {
                    name = "Онлайн-интервью";
                }
                else if (type == ManualSettings.Interview.Type.OTHER)
                {
                    name = "Интервью другого типа";
                }
                else
                {
                    name = "Неизвестный тип интервью";
                }
            }

            public override string ToString()
            {
                return $"[{type}] cold: {cold} hot: {hot}";
            }

            internal static OldInterviewCount[] GetCountsFromLogs(ApplicantWorklogs[] applicantWorklogs, Applicant[] applicants)
            {
                var counts = new Dictionary<int, OldInterviewCount>();
                foreach (var type in settings.interview_types)
                {
                    counts.Add(type.id, new OldInterviewCount(0, 0, type.type));
                }

                for (int a = 0; a < applicants.Length; a++)
                {
                    for (int w = 0; w < applicantWorklogs[a].logs.Length; w++)
                    {
                        if (applicantWorklogs[a].logs[w].calendarEvent != null)
                        {
                            try
                            {
                                var interviewTypeId = applicantWorklogs[a].logs[w].calendarEvent.interviewType;
                                if (applicants[a].IsHot(applicantWorklogs[a].logs))
                                {
                                    counts[interviewTypeId].hot++;
                                }
                                else
                                {
                                    counts[interviewTypeId].cold++;
                                }
                            }
                            catch (KeyNotFoundException e) { }
                        }
                    }
                }
                return counts.Values.ToArray();
            } 
        }

        internal class InterviewCount
        {
            internal int cold;
            internal int hot;
            internal Type type;

            internal InterviewCount(int cold, int hot, Type type)
            {
                this.cold = cold;
                this.hot = hot;
                this.type = type;
            }

            public override string ToString()
            {
                return $"[{type}] cold: {cold} hot: {hot}";
            }

            internal enum Type
            {
                PHONE,
                HR,
                FINAL
            }
        }
    }
}
