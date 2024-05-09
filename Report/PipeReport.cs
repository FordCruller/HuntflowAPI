using HuntflowAPI.Data.Huntflow;
using System;
using System.Collections.Generic;
using static HuntflowAPI.Order.PipeReport.ApplicantMigrationElement;
using static HuntflowAPI.Order.PipeReport.RejectReasonElement;
using static HuntflowAPI.Order.PipeReport.ApplicantMigrationElement.StepPeriod;
using System.Linq;
using System.Runtime.InteropServices;

namespace HuntflowAPI.Order
{
    public class PipeReport
    {
        public ApplicantMigrationElement[] migrationElements;
        public RejectReasonElement[] rejectReasonElements;

        public Week[] weeks;
        public string[] stepNames;
        public string[] rejectReasonNames;

        public Dictionary<int, ApplicantStep> stepDictinioary;

        public PipeReport(ApplicantsOrderData data)
        {
            ApplicantMigrationElement[] migrationElements = new ApplicantMigrationElement[data.steps.Length];
            RejectReasonElement[] rejectReasonElements = new RejectReasonElement[data.rejectionReasons.Length];

            var startDate = FindStartWorkDate(data.worklogs);
            var endDate = FindEndWorkDate(data.worklogs);
            weeks = Week.GetCalendarWeeks(startDate, endDate);
            stepDictinioary = ApplicantStep.GetDictionary(data.steps);

            stepNames = data.steps.Select(p => p.name).ToArray();
            rejectReasonNames = data.rejectionReasons.Select(p => p.name).ToArray();

            ApplicantSteps[] applicantSteps = new ApplicantSteps[data.applicants.Length];
            RejectionReasonPeriod.ApplicantRejectReason[] applicantRejectReasons = new RejectionReasonPeriod.ApplicantRejectReason[data.applicants.Length];
            for (int i = 0; i < data.worklogs.Length; i++)
            {
                applicantSteps[i] = new ApplicantSteps(data.applicants[i].IsHot(data.worklogs[i].logs), GetStructureSteps(data.steps, data.worklogs[i].logs));
                applicantRejectReasons[i] = new RejectionReasonPeriod.ApplicantRejectReason(data.applicants[i], data.worklogs[i].logs);
            }

            for (int i = 0; i < data.steps.Length; i++)
            {
                var periods = new StepPeriod[weeks.Length];
                for (int p = 0; p < periods.Length; p++)
                {
                    periods[p] = new StepPeriod(applicantSteps, weeks[p], data.steps[i].id);
                }
                migrationElements[i] = new ApplicantMigrationElement(data.steps[i].name, periods);
            }

            for (int i = 0; i < data.rejectionReasons.Length; i++)
            {
                var periods = new RejectionReasonPeriod[weeks.Length];
                for (int p = 0; p < periods.Length; p++)
                {
                    periods[p] = new RejectionReasonPeriod(applicantRejectReasons, weeks[p], data.rejectionReasons[i].id);
                }
                rejectReasonElements[i] = new RejectReasonElement(data.rejectionReasons[i].name, periods);
            }

            this.migrationElements = migrationElements;
            this.rejectReasonElements = rejectReasonElements;
        }

        public class RejectReasonElement
        {
            internal string rejectReasonName;
            internal RejectionReasonPeriod[] periods;

            public RejectReasonElement(string rejectReasonName, RejectionReasonPeriod[] periods)
            {
                this.rejectReasonName = rejectReasonName;
                this.periods = periods;
            }

            public class RejectionReasonPeriod
            {
                public int hotRejectionReasons { get; set; }
                public int coldRejectionReasons { get; set; }

                public RejectionReasonPeriod(ApplicantRejectReason[] reasons, Week week, int reasonId)
                {
                    foreach (var reason in reasons)
                    {
                        if (week.Contains(reason.date))
                        {
                            if (reason.id == reasonId)
                            {
                                if (reason.isHot)
                                {
                                    hotRejectionReasons++;
                                }
                                else
                                {
                                    coldRejectionReasons++;
                                }
                            }
                        }
                    }
                }

                public class ApplicantRejectReason
                {
                    internal bool isHot;
                    internal int id;
                    internal DateTime date;
                    private static ManualSettings settings = ManualSettings.GetInstance();

                    public ApplicantRejectReason(Applicant applicant, Worklog[] worklogs)
                    {
                        isHot = applicant.IsHot(worklogs);
                        foreach (var log in worklogs)
                        {
                            if (log.statusId.Equals(settings.reject_id))
                            {
                                id = log.rejectionReasonId;
                                date = log.created;
                                break;
                            }
                        }
                    }
                }
            }
        }

        public class ApplicantMigrationElement
        {
            public string stepName { get; set; }
            public StepPeriod[] periods { get; set; }

            public ApplicantMigrationElement(string stepName, StepPeriod[] periods)
            {
                this.stepName = stepName;
                this.periods = periods;
            }

            public class StepPeriod
            {
                public int hotApplicantsCount { get; set; }
                public int coldApplicantsCount { get; set; }
                
                public StepPeriod(ApplicantSteps[] steps, Week week, int stepId)
                {
                    foreach(var applicantSteps in steps) 
                    {
                        if (applicantSteps.Contains(stepId))
                        {
                            int index = ApplicantSteps.IndexOf(applicantSteps.steps, stepId);
                            if (week.Contains(applicantSteps.steps[index].date))
                            {
                                if (applicantSteps.isHot)
                                {
                                    hotApplicantsCount++;
                                }
                                else
                                {
                                    coldApplicantsCount++;
                                }
                            }
                        }
                    }
                }

                public class ApplicantSteps
                {
                    internal bool isHot;
                    internal Step[] steps;

                    public ApplicantSteps(bool isHot, Step[] steps)
                    {
                        this.isHot = isHot;
                        this.steps = steps;
                    }

                    internal bool Contains(int stepId)
                    {
                        foreach (var step in steps)
                        {
                            if (step.id == stepId)
                            {
                                return true;
                            }
                        }
                        return false;
                    }

                    public static int IndexOf(Step[] steps, int stepId)
                    {
                        for (int i = 0; i < steps.Length; i++)
                        {
                            if (steps[i].id == stepId)
                            {
                                return i;
                            }
                        }
                        return -1;
                    }
                }

                public class Step
                {
                    public DateTime date;
                    public int id;

                    internal Step(DateTime date, int stepId)
                    {
                        this.date = date;
                        this.id = stepId;
                    }
                }
            }

            public static StepPeriod.Step[] GetStructureSteps(ApplicantStep[] steps, Worklog[] logs)
            {
                var stepsDictiniory = ApplicantStep.GetDictionary(steps);

                //Get only step-by-step array
                List<StepPeriod.Step> stepsByStepArray = new List<StepPeriod.Step>();
                for (int i = logs.Length - 1; i >= 0; i--)
                {
                    if (logs[i].statusId != -1 && stepsDictiniory[logs[i].statusId].stepByStep && !(stepsByStepArray.Count > 0 && stepsByStepArray[stepsByStepArray.Count - 1].id == logs[i].statusId))
                    {
                        stepsByStepArray.Add(new StepPeriod.Step(logs[i].created, logs[i].statusId));
                    }
                }

                //Delete old steps
                Stack<StepPeriod.Step> currentSteps = new Stack<StepPeriod.Step>();
                if (stepsByStepArray.Count > 0)
                {
                    currentSteps.Push(stepsByStepArray[0]);
                }
                for (int i = 1; i < stepsByStepArray.Count; i++)
                {
                    if (!ApplicantStep.MaintainsSequence(currentSteps.Peek().id, stepsByStepArray[i].id))
                    {
                        currentSteps.Clear();
                    }
                    currentSteps.Push(stepsByStepArray[i]);
                }
                StepPeriod.Step[] stepsArray = currentSteps.ToArray();
                Array.Reverse(stepsArray);

                var linearSteps = ManualSettings.GetInstance().step_by_step;
                List<StepPeriod.Step> allSteps = new List<StepPeriod.Step>();
                if (stepsArray.Length > 0)
                {
                    allSteps.Add(stepsArray[0]);
                }
                if (stepsArray.Length > 1)
                {
                    for (int i = 1; i < stepsArray.Length; i++)
                    {
                        foreach (var stepId in linearSteps)
                        {
                            if (Array.IndexOf(linearSteps, stepId) > Array.LastIndexOf(linearSteps, stepsArray[i - 1].id) && Array.IndexOf(linearSteps, stepId) < Array.LastIndexOf(linearSteps, stepsArray[i].id))
                            {
                                allSteps.Add(new StepPeriod.Step(stepsArray[i].date, stepId));
                            }
                        }
                        allSteps.Add(stepsArray[i]);
                    }
                }


                if (allSteps != null && allSteps.Count > 0)
                {
                    var firstStep = allSteps[0].id;
                    if (Array.IndexOf(linearSteps, firstStep) != 0)
                    {
                        DateTime date = allSteps[0].date;
                        allSteps.Reverse();
                        for (int i = Array.IndexOf(linearSteps, firstStep) - 1; i >= 0; i--)
                        {
                            allSteps.Add(new Step(date, linearSteps[i]));
                        }
                        allSteps.Reverse();
                    }
                }

                for (int i = logs.Length - 1; i >= 0; i--)
                {
                    if (logs[i].statusId != -1 && !stepsDictiniory[logs[i].statusId].stepByStep && !(allSteps.Count > 0 && allSteps[allSteps.Count - 1].id == logs[i].statusId))
                    {
                        allSteps.Add(new StepPeriod.Step(logs[i].created, logs[i].statusId));
                    }
                }

                return allSteps.ToArray();
            }

            public class Week
            {
                public DateTime startDay;
                public DateTime endDay;

                public Week(DateTime startDay, DateTime endDay)
                {
                    this.startDay = startDay;
                    this.endDay = endDay;
                }

                public static Week[] GetCalendarWeeks(DateTime startDate, DateTime endDate)
                {
                    startDate = startDate.Date;
                    
                    var weeks = new List<Week>();
                    DateTime current = startDate.AddDays(-(int)startDate.DayOfWeek + 1);
                    
                    while (current <= endDate)
                    {
                        Week week = new Week(current, current.AddDays(6).AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999));
                        weeks.Add(week);
                        current = current.AddDays(7);
                    }

                    return weeks.ToArray();
                }

                public bool Contains(DateTime date)
                {
                    return date >= startDay && date < endDay;
                }
            }
        }

        private static DateTime FindStartWorkDate(ApplicantWorklogs[] worklogs)
        {
            var startDate = DateTime.Today;
            foreach (var applicantWorklog in worklogs)
            {
                foreach (var log in applicantWorklog.logs)
                {
                    if (log.created < startDate)
                    {
                        startDate = log.created;
                    }
                }
            }
            return startDate;
        }

        private static DateTime FindEndWorkDate(ApplicantWorklogs[] worklogs)
        {
            var currentDate = DateTime.Today;
            var lastDate = DateTime.MinValue;
            foreach (var applicantWorklog in worklogs)
            {
                foreach (var log in applicantWorklog.logs)
                {
                    if (log.created > lastDate)
                    {
                        lastDate = log.created;
                    }
                }
            }
            lastDate = lastDate.AddDays(7);
            if (lastDate < currentDate)
            {
                return lastDate;
            }
            else
            {
                return currentDate;
            }
        }
    }
}
