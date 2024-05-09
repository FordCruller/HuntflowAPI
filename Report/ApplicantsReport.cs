using HuntflowAPI.Data.Huntflow;
using System;
using System.Collections.Generic;
using System.Text;

namespace HuntflowAPI.Data
{
    public class ApplicantsReport
    {
        public PipeOrderElement[] elements;
        public ApplicantStep[] steps;

        public ApplicantsReport(ApplicantsOrderData data)
        {
            var pipeOrderElements = new PipeOrderElement[data.applicants.Length];
            var dictionarySteps = ApplicantStep.GetDictionary(data.steps);
            var dictionaryRejectionReasons = RejectionReason.GetDictionary(data.rejectionReasons);
            var dictionaryTags = Tag.GetDictionary(data.tags);
            var settings = ManualSettings.GetInstance();

            steps = data.steps;

            for (int i = 0; i < pipeOrderElements.Length; i++)
            {
                var worklog = data.worklogs[i];
                var applicant = data.applicants[i];
                var resume = data.resumes[i];

                int hhIndex = applicant.GetHHExternalIndex();

                string name = "";
                if (applicant.lastName != null)
                {
                    name += applicant.lastName + " ";
                }
                if (applicant.firstName != null)
                {
                    name += applicant.firstName + " ";
                }
                if (applicant.middleName != null)
                {
                    name += applicant.middleName + " ";
                }
                if (name.Length > 0)
                {
                    name = name.Substring(0, name.Length - 1);
                }
                
                string wantedSalary = "";
                if (applicant.money != null)
                {
                    wantedSalary += applicant.money;
                    wantedSalary = wantedSalary.Replace("RUR", "руб.");
                }

                string city = "";
                if (resume != null && resume.data != null && resume.data.area != null && resume.data.area.name != null)
                {
                    city += resume.data.area.name;
                }

                bool isHot = applicant.IsHot(worklog.logs);

                string source = "huntflow";
                if (hhIndex != -1)
                {
                    source = "hh.ru";
                }

                DateTime createDate = applicant.created;

                CommunicationChannel[] communicationChannels = GetFoundCommunicationChannelByKeywords(worklog.logs);
                string communicationChannelsString = "";
                for (int c = 0; c < communicationChannels.Length; c++)
                {
                    communicationChannelsString += ConvertToString(communicationChannels[c]);
                    if (c < communicationChannels.Length - 1) { communicationChannelsString += ", "; }
                }

                int currentStepId = worklog.logs[0].statusId;

                ApplicantStep currentStep = null;
                try
                {
                    currentStep = dictionarySteps[currentStepId];
                }
                catch (KeyNotFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                }

                string rejectionReason = "";
                if (currentStepId == settings.reject_id)
                {
                    rejectionReason += GetRejectionReasonFromWorklog(dictionaryRejectionReasons, worklog.logs[0]);
                }

                string commentsLog = "";

                Worklog[] reverceLogs = (Worklog[])worklog.logs.Clone();
                Array.Reverse(reverceLogs);

                foreach (var log in reverceLogs)
                {
                    commentsLog += $"[{log.created.ToString("dd.MM.yyyy HH:mm")}]: ";
                    if (log.statusId != -1)
                    {
                        commentsLog += $"{dictionarySteps[log.statusId].name} - ";
                    }
                    if (log.comment != null)
                    {
                        commentsLog += $"{log.comment};";
                    }
                    if (commentsLog[commentsLog.Length - 3] == '-')
                    {
                        commentsLog = commentsLog.Substring(0, commentsLog.Length - 4) + ";";
                    }
                    commentsLog += "\n";
                }

                Dictionary<int, Step> steps = GetApplicantSteps(dictionarySteps, worklog.logs);

                var tags = "";
                string[] tagsArray = new string[applicant.tags.Length];
                if (dictionaryTags != null)
                {
                    for (int t = 0; t < tagsArray.Length; t++)
                    {
                        try
                        {
                            tagsArray[t] = dictionaryTags[applicant.tags[t].tag].name;
                        }
                        catch (KeyNotFoundException ex) { Console.WriteLine(ex.Message + $"Тег {applicant.tags[t].tag} не был найден."); }
                    }
                }
                tags = JoinStrings(tagsArray);

                //string url = @"https://huntflow.ru/my/" +  + "cnordhr#/vacancy/3626110/filter/workon/id/47183371";

                pipeOrderElements[i] = new PipeOrderElement(0, name, wantedSalary, city, isHot, source, createDate, communicationChannelsString, currentStep, rejectionReason, commentsLog, tags, steps);
            }

            Array.Sort(pipeOrderElements, (a1, a2) => a1.createDate.CompareTo(a2.createDate));
            for(int i = 0; i < pipeOrderElements.Length; i++)
            {
                pipeOrderElements[i].id = i + 1;
            }

            this.elements = pipeOrderElements;
        }

        private static string JoinStrings(string[] strings)
        {
            if (strings == null || strings.Length == 0)
            {
                return string.Empty;
            }

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < strings.Length; i++)
            {
                result.Append(strings[i]);
                if (i < strings.Length - 1)
                {
                    result.Append(", ");
                }
            }

            return result.ToString();
        }

        private static string[] WHATSAPP_KEYWORDS = new string[] { "WHATSAPP", "ВОТСАП", "ВА", "WA", "ВОТС", "ВАЦАП", "ВОЦАП", "ВАЦАПП", "ВОЦАПП", "ВЦ" };
        private static string[] TELEGRAM_KEYWORDS = new string[] { "TELEGRAM", "ТЕЛЕГРАМ", "ТЕЛЕГРАММ", "ТГ", "TG", "ТЕЛЕГА" };
        private static string[] HEADHUNTER_KEYWORDS = new string[] { "HHRU", "HH", "HEADHUNTER", "ХХ", "ХД", "ХЕДХАНТЕР", "ХЭДХАНТЕР" };
        private static string[] PHONE_KEYWORDS = new string[] { "ТЕЛ", "ТЕЛЕФОН", "СВЯЗЬ", "НОМЕР", "СБРОСИЛ" };
        private static string[] MAIL_KEYWORDS = new string[] { "ЭП", "ЭЛЕКТРОНКА", "ПОЧТА", "ЕМЕЙЛ", "E-MAIL", "Е-МЕЙЛ", "MAIL", "МЕЙЛ", "ЭЛП", "ЭМЛ" };


        private static Dictionary<int, Step> GetApplicantSteps(Dictionary<int, ApplicantStep> dictionaryApplicantSteps, Worklog[] logs)
        {
            HashSet<int> statusInds = new HashSet<int>();
            foreach (var log in logs)
            {
                if (log.statusId != -1)
                {
                    statusInds.Add(log.statusId);
                }
            }

            var steps = new List<Step>();
            foreach (var status in statusInds)
            {
                foreach (var log in logs)
                {
                    if (log.statusId == status)
                    {
                        steps.Add(new Step(log.statusId, dictionaryApplicantSteps[log.statusId].name, log.created));
                        break;
                    }
                }
            }

            var stepsDictionary = new Dictionary<int, Step>();
            for (int i = 0; i < steps.Count; i++)
            {
                stepsDictionary[steps[i].id] = steps[i];
            }

            return stepsDictionary;
        }

        private static string GetRejectionReasonFromWorklog(Dictionary<int, RejectionReason> dictionaryRejectionReasons, Worklog log)
        {
            string rejectionReason;
            if (log.rejectionReasonId == -1)
            {
                if (log.comment == null || log.comment.Equals(""))
                {
                    rejectionReason = "По неизвестной причине";
                }
                else
                {
                    rejectionReason = log.comment;
                }
            }
            else
            {
                try
                {
                    rejectionReason = dictionaryRejectionReasons[log.rejectionReasonId].name;
                }
                catch (KeyNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    if (log.comment == null || log.comment.Equals(""))
                    {
                        rejectionReason = "По неизвестной причине";
                    }
                    else
                    {
                        rejectionReason = log.comment;
                    }
                }
            }
            return rejectionReason;
        }

        private static string ConvertToString(CommunicationChannel channel)
        {
            if (channel == CommunicationChannel.PHONE)
            {
                return "телефон";
            }
            else if (channel == CommunicationChannel.MAIL)
            {
                return "почта";
            }
            else if (channel == CommunicationChannel.WHATSAPP)
            {
                return "whatsapp";
            }
            else if (channel == CommunicationChannel.TELEGRAM)
            {
                return "telegram";
            }
            else if (channel == CommunicationChannel.HEADHUNTER)
            {
                return "hh.ru";
            }
            return "неизвестный источник";
        }

        private static CommunicationChannel[] GetFoundCommunicationChannelByKeywords(Worklog[] logs)
        {
            string comments = "";
            foreach (var log in logs)
            {
                comments += $"{log} {log.comment} ";
            }

            char[] charsToRemove = { ',', '.', ':', ';', '-', '_' };
            comments = RemoveCharsFromString(comments, charsToRemove);
            string[] words = comments.Split(' ');
            return CheckChannelMembershipByLevenshtein(words, 1);
        }

        private static CommunicationChannel[] CheckChannelMembershipByLevenshtein(string[] wordArray, int distance)
        {
            var channels = new List<CommunicationChannel>();

            if (CompareByLevenshteinDistance(PHONE_KEYWORDS, wordArray, distance))
            {
                channels.Add(CommunicationChannel.PHONE);
            }
            if (CompareByLevenshteinDistance(HEADHUNTER_KEYWORDS, wordArray, distance))
            {
                channels.Add(CommunicationChannel.HEADHUNTER);
            }
            if (CompareByLevenshteinDistance(TELEGRAM_KEYWORDS, wordArray, distance))
            {
                channels.Add(CommunicationChannel.TELEGRAM);
            }
            if (CompareByLevenshteinDistance(WHATSAPP_KEYWORDS, wordArray, distance))
            {
                channels.Add(CommunicationChannel.WHATSAPP);
            }
            if (CompareByLevenshteinDistance(MAIL_KEYWORDS, wordArray, distance))
            {
                channels.Add(CommunicationChannel.MAIL);
            }

            return channels.ToArray();
        }

        private static bool CompareByLevenshteinDistance(string[] dataArray, string[] parseArray, int distance)
        {
            foreach (var parseWord in parseArray)
            {
                foreach (var dataWord in dataArray)
                {
                    if ((parseWord.Length > 3 && LevenshteinDistance(dataWord, parseWord.ToUpper()) <= distance) || (parseWord.Length < 4 && parseWord.ToUpper().Equals(dataWord)))
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }

        private static string RemoveCharsFromString(string input, char[] charsToRemove)
        {
            foreach (char c in charsToRemove)
            {
                input = input.Replace(c.ToString(), string.Empty);
            }
            return input;
        }

        private static int LevenshteinDistance(string s, string t)
        {
            int m = s.Length;
            int n = t.Length;
            int[,] d = new int[m + 1, n + 1];

            for (int i = 0; i <= m; i++)
                d[i, 0] = i;

            for (int j = 0; j <= n; j++)
                d[0, j] = j;

            for (int j = 1; j <= n; j++)
            {
                for (int i = 1; i <= m; i++)
                {
                    int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }

            return d[m, n];
        }

        public class PipeOrderElement
        {
            public int id;
            public string name;
            public string wantedSalary;
            public string city;
            public bool isHot;
            public string source;
            public DateTime createDate;
            public string communicationChannels;
            public ApplicantStep currentStep;
            public string rejectionReason;
            public string commentsLog;
            public Dictionary<int, Step> steps;
            public string tags;

            internal PipeOrderElement(int id, string name, string wantedSalary, string city, bool isHot, string source, DateTime createDate, string communicationChannel, ApplicantStep currentStep, string rejectionReason, string commentsLog, string tags, Dictionary<int, Step> steps)
            {
                this.id = id;
                this.name = name;
                this.wantedSalary = wantedSalary;
                this.city = city;
                this.isHot = isHot;
                this.source = source;
                this.createDate = createDate;
                this.communicationChannels = communicationChannel;
                this.currentStep = currentStep;
                this.rejectionReason = rejectionReason;
                this.commentsLog = commentsLog;
                this.steps = steps;
                this.tags = tags;
            }

            public override string ToString()
            {
                string str = "";
                str += $"[{id}] ";
                if (name != null) { str += $"{name}, "; }
                if (wantedSalary != null) { str += $"{wantedSalary}, "; }
                if (city != null) { str += $"{city}, "; }
                str += isHot ? "теплый, " : "холодный, ";
                if (source != null) { str += $"{source}, "; }
                if (createDate != null) { str += $"{createDate}, "; }
                if (communicationChannels != null) { str += $"{communicationChannels}, "; }
                if (currentStep != null) { str += $"{currentStep}, "; }
                if (rejectionReason != null) { str += $"{rejectionReason}, "; }
                //if (commentsLog != null) { str += $"{commentsLog}, "; }

                foreach (var step in steps)
                {
                    if (step.Value != null) { str += $"[{step.Value.name}]:{step.Value.date}, "; }
                }
                return str;
            }
        }

        public enum CommunicationChannel
        {
            PHONE,
            TELEGRAM,
            WHATSAPP,
            MAIL,
            HEADHUNTER
        }

        public class Step
        {
            public int id { get; set; }
            public string name { get; set; }
            public DateTime date { get; set; }

            public Step(int id, string name, DateTime date)
            {
                this.id = id;
                this.name = name;
                this.date = date;
            }
        }
    }
}
