using HuntflowAPI.Data.Huntflow;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using static HuntflowAPI.GetRequest;
using static HuntflowAPI.HuntflowApplicantsResume;
using static HuntflowAPI.HuntflowApplicantWorklogs;
using static HuntflowAPI.HuntflowRejectionReasons;
using static HuntflowAPI.HuntflowResumeSources;
using static HuntflowAPI.HuntflowVacancyList;

namespace HuntflowAPI.Data
{
    public class HuntflowData
    {
        AuthToken authToken;
        HuntflowRequest huntflow;
        
        static Serializer serializer = new Serializer();
        static Deserializer deserializer = new Deserializer();

        static HuntflowData instance;

        //Написать аргумент, который принимает стейт, который определяет будет ли подгружаться информация из сети если не найдены объекты в кэше
        private HuntflowData()
        {
            string executingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string tokenPath = Path.GetFullPath(Path.Combine(executingPath, @"..\..\Data\Huntflow\huntflow_auth_token.json"));

            string json = System.IO.File.ReadAllText(tokenPath);
            authToken = new AuthToken(json);

            huntflow = new HuntflowRequest(authToken);
        }

        public static HuntflowData GetInstance()
        {
            if (instance == null)
            {
                instance = new HuntflowData();
            }
            return instance;
        }

        public AuthToken GetToken()
        {
            return authToken;
        }

        public Vacancy GetVacancy(Source source, int accountId, int vacancyId)
        {
            string cacheFileName = $"vacancies_{accountId}";
            if (source == Source.HUNTFLOW)
            {
                return new Vacancy(huntflow.GetAVacancy(accountId, vacancyId));
            }
            else if (source == Source.CACHE)
            {
                HuntflowVacancyList huntflowVacancies = deserializer.Deserialize<HuntflowVacancyList>(cacheFileName);
                HuntflowVacancy targetVacancy = null;
                foreach (var huntflowVacancy in huntflowVacancies.items)
                {
                    if (huntflowVacancy.id == vacancyId)
                    {
                        targetVacancy = huntflowVacancy;
                    }
                }

                if (targetVacancy == null)
                {
                    throw new CacheObjectNotFoundException("Vacancy not found in cache-file.");
                }
                else
                {
                    return new Vacancy(targetVacancy);
                }
            }

            return null;
        }

        public Vacancy[] GetAllVacancies(Source source, int accountId, bool mine, VacancyState[] vacancyStates)
        {
            string cacheFileName = $"vacancies_{accountId}";
            if (source == Source.HUNTFLOW)
            {
                List<HuntflowVacancy> allHuntflowVacancyItems = new List<HuntflowVacancy>();
                
                var huntflowVacancyList = huntflow.GetAllVacancies(accountId, 100, 1, mine, vacancyStates);
                allHuntflowVacancyItems.AddRange(huntflowVacancyList.items);

                int totalPages = huntflowVacancyList.total_pages;
                for (int i = 2; i <= totalPages; i++)
                {
                    allHuntflowVacancyItems.AddRange(huntflow.GetAllVacancies(accountId, 100, i, mine, vacancyStates).items);
                }


                huntflowVacancyList = new HuntflowVacancyList(allHuntflowVacancyItems.ToArray());

                serializer.Serialize(huntflowVacancyList, cacheFileName);
                return Vacancy.Convert(huntflowVacancyList);
            }
            else if (source == Source.CACHE)
            {
                HuntflowVacancyList huntflowVacancyList = deserializer.Deserialize<HuntflowVacancyList>(cacheFileName);
                return Vacancy.Convert(huntflowVacancyList);
            }
            return null;
        }

        public Organization[] GetAllOrganizations(Source source)
        {
            string cacheFileName = "organizations";
            if (source == Source.HUNTFLOW)
            {
                var huntflowOrganizations = huntflow.GetAllAvailableOrganizations();
                serializer.Serialize(huntflowOrganizations, cacheFileName);
                
                return Organization.Convert(huntflowOrganizations);
            }
            else if (source == Source.CACHE)
            {
                HuntflowOrganizations huntflowOrganizations = deserializer.Deserialize<HuntflowOrganizations>(cacheFileName);
                return Organization.Convert(huntflowOrganizations);
            }
            return null;
        }

        public Applicant[] GetAllApplicants(Source source, int accountId, int vacancyId)
        {
            string cacheFileName = $"applicants_{accountId}_{vacancyId}";
            if (source == Source.HUNTFLOW)
            {
                List<HuntflowApplicant> allHuntflowApplicantItems = new List<HuntflowApplicant>();
                var huntflowApplicants = huntflow.GetAllApplicants(accountId, 30, 1, vacancyId);
                allHuntflowApplicantItems.AddRange(huntflowApplicants.items);

                int totalPages = huntflowApplicants.total_pages;
                for (int i = 2; i <= totalPages; i++)
                {
                    allHuntflowApplicantItems.AddRange(huntflow.GetAllApplicants(accountId, 30, i, vacancyId).items);
                }

                huntflowApplicants = new HuntflowApplicants(allHuntflowApplicantItems.ToArray());

                serializer.Serialize(huntflowApplicants, cacheFileName);

                return Applicant.Convert(huntflowApplicants);
            }
            else if (source == Source.CACHE)
            {
                var huntflowApplicants  = deserializer.Deserialize<HuntflowApplicants>(cacheFileName);
                return Applicant.Convert(huntflowApplicants);
            }
            return null;
        }

        public ApplicantWorklogs[] GetAllWorklogs(Source source, int accountId, int vacancyId, int[] applicantIds, LogType[] types, bool personal)
        {
            string cacheFileName = $"worklogs_{accountId}_{vacancyId}";
            if (source == Source.HUNTFLOW)
            {
                ApplicantWorklogs[] applicantWorklogs = new ApplicantWorklogs[applicantIds.Length];
                for(int a = 0; a < applicantIds.Length; a++)
                {
                    List<HuntflowApplicantWorklog> allHuntflowApplicantWorklogsItems = new List<HuntflowApplicantWorklog>();
                    var huntflowWorklogs = huntflow.GetAnApplicantsWorklog(accountId, applicantIds[a], types, 30, 1, personal, vacancyId);
                    allHuntflowApplicantWorklogsItems.AddRange(huntflowWorklogs.items);

                    int totalPages = huntflowWorklogs.total_pages;
                    for (int i = 2; i <= totalPages; i++)
                    {
                        allHuntflowApplicantWorklogsItems.AddRange(huntflow.GetAnApplicantsWorklog(accountId, applicantIds[a], types, 30, i, personal, vacancyId).items);
                    }

                    huntflowWorklogs = new HuntflowApplicantWorklogs(allHuntflowApplicantWorklogsItems.ToArray());
                    applicantWorklogs[a] = new ApplicantWorklogs(applicantIds[a], Worklog.Convert(huntflowWorklogs));
                }

                serializer.Serialize(applicantWorklogs, cacheFileName);
                return applicantWorklogs;
            }
            else if (source == Source.CACHE)
            {
                var applicantWorklogs = deserializer.Deserialize<ApplicantWorklogs[]>(cacheFileName);
                return applicantWorklogs;
            }
            return null;
        }

        public Resume GetApplicantResume(Source source, int accountId, int applicantId, int externalId)
        {
            string cacheFileName = $"resume_{accountId}_{applicantId}_{externalId}";
            if (source == Source.HUNTFLOW)
            {
                var huntflowApplicants = huntflow.GetAnApplicantsResume(accountId, applicantId, externalId);
                serializer.Serialize(huntflowApplicants, cacheFileName);

                return new Resume(huntflowApplicants);
            }
            else if (source == Source.CACHE)
            {
                var huntflowApplicants = deserializer.Deserialize<HuntflowApplicantsResume>(cacheFileName);
                return new Resume(huntflowApplicants);
            }
            return null;
        }

        public ApplicantStep[] GetAllApplicantSteps(Source source, int accountId)
        {
            string cacheFileName = $"steps_{accountId}";
            if (source == Source.HUNTFLOW)
            {
                var huntflowRecruitmentStatuses = huntflow.GetAllRecruitmentStatuses(accountId);
                serializer.Serialize(huntflowRecruitmentStatuses, cacheFileName);

                return ApplicantStep.Convert(huntflowRecruitmentStatuses);
            }
            else if (source == Source.CACHE)
            {
                var huntflowRecruitmentStatuses = deserializer.Deserialize<HuntflowRecruitmentStatuses>(cacheFileName);
                return ApplicantStep.Convert(huntflowRecruitmentStatuses);
            }
            return null;
        }

        public RejectionReason[] GetAllRejectionReasons(Source source, int accountId)
        {
            string cacheFileName = $"rejection_reasons_{accountId}";
            if (source == Source.HUNTFLOW)
            {
                var huntflowRejectionReasons = huntflow.GetAllRejectionReasons(accountId);
                serializer.Serialize(huntflowRejectionReasons, cacheFileName);

                return RejectionReason.Convert(huntflowRejectionReasons);
            }
            else if (source == Source.CACHE)
            {
                var huntflowRejectionReasons = deserializer.Deserialize<HuntflowRejectionReasons>(cacheFileName);
                return RejectionReason.Convert(huntflowRejectionReasons);
            }
            return null;
        }

        public Tag[] GetAllTags(Source source, int accountId)
        {
            string cacheFileName = $"tags_{accountId}";
            if (source == Source.HUNTFLOW)
            {
                var huntflowTags = huntflow.GetAllTags(accountId);
                serializer.Serialize(huntflowTags, cacheFileName);

                return Tag.Convert(huntflowTags);
            }
            else if (source == Source.CACHE)
            {
                var huntflowTags = deserializer.Deserialize<HuntflowTags>(cacheFileName);
                return Tag.Convert(huntflowTags);
            }
            return null;
        }

        public class Converter
        {
            private Converter() { }
            public static T[] ConvertArray<T>(object[] objects)
            {
                T[] result = new T[objects.Length];

                for (int i = 0; i < objects.Length; i++)
                {
                    result[i] = (T)Activator.CreateInstance(typeof(T), new object[] { objects[i] });
                }

                return result;
            }
        }

        public static void UpdateCache(HuntflowEntity[] huntflowEntities, int accountId)
        {
            var data = GetInstance();
            foreach (var entity in huntflowEntities) 
            {
                if (entity == HuntflowEntity.ORGANIZATIONS)
                {
                    data.GetAllOrganizations(Source.HUNTFLOW);
                }
                else if (entity == HuntflowEntity.REJECTION_REASONS)
                {
                    data.GetAllRejectionReasons(Source.HUNTFLOW, accountId);
                }
                else if (entity == HuntflowEntity.STEPS)
                {
                    data.GetAllApplicantSteps(Source.HUNTFLOW, accountId);
                }
                else if (entity == HuntflowEntity.VACANCIES)
                {
                    data.GetAllVacancies(Source.HUNTFLOW, accountId, false, new VacancyState[] { VacancyState.OPEN });
                }
                else if (entity == HuntflowEntity.TAGS)
                {
                    data.GetAllTags(Source.HUNTFLOW, accountId);
                }
            }
        }
        public static void UpdateCache(int accountId)
        {
            UpdateCache((HuntflowEntity[])Enum.GetValues(typeof(HuntflowEntity)), accountId);
        }

    }

    public enum HuntflowEntity
    {
        ORGANIZATIONS,
        STEPS,
        REJECTION_REASONS,
        VACANCIES,
        TAGS
    }

    public enum Source
    {
        CACHE,
        HUNTFLOW
    }

    public class CacheObjectNotFoundException : Exception
    {
        public CacheObjectNotFoundException() { }

        public CacheObjectNotFoundException(string message)
            : base(message) { }

        public CacheObjectNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
