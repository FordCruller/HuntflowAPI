using HuntflowAPI.Data;
using HuntflowAPI.Data.Huntflow;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HuntflowAPI
{
    public class ReportData
    {
        protected int accountId;
        protected int vacancyId;

        public Vacancy vacancy;

        public Applicant[] applicants;
        public ApplicantWorklogs[] worklogs;
        public RejectionReason[] rejectionReasons;
        public ApplicantStep[] steps;
        public Resume[] resumes;
        public Tag[] tags;
    }

    public class ApplicantsOrderData : ReportData
    {
        private ApplicantsOrderData(int accountId, int vacancyId, Vacancy vacancy, Applicant[] applicants, ApplicantWorklogs[] worklogs, RejectionReason[] rejectionReasons, ApplicantStep[] steps, Resume[] resumes, Tag[] tags)
        {
            this.accountId = accountId;
            this.vacancyId = vacancyId;
            this.applicants = applicants;
            this.worklogs = worklogs;
            this.rejectionReasons = rejectionReasons;
            this.steps = steps;
            this.resumes = resumes;
            this.tags = tags;
            this.vacancy = vacancy;
        }

        public static ApplicantsOrderData FromHuntflow(Data.Source dataSource, int accountId, int vacancyId)
        {
            HuntflowData data = HuntflowData.GetInstance();
            Applicant[] applicants = data.GetAllApplicants(dataSource, accountId, vacancyId);
            ApplicantWorklogs[] worklogs = data.GetAllWorklogs(dataSource, accountId, vacancyId, Applicant.GetApplicantIdsArray(applicants), new GetRequest.LogType[] { GetRequest.LogType.COMMENT, GetRequest.LogType.STATUS, GetRequest.LogType.RESPONSE }, false);

            Resume[] resumes = new Resume[applicants.Length];
            for (int i = 0; i < applicants.Length; i++)
            {
                Applicant applicant = applicants[i];

                int hhIndex = applicant.GetHHExternalIndex();

                if (hhIndex != -1)
                {
                    try
                    {
                        resumes[i] = data.GetApplicantResume(Data.Source.CACHE, accountId, applicant.id, applicant.external[hhIndex].id);
                        continue;
                    }
                    catch (FileNotFoundException e)
                    {
                        Console.WriteLine(e.Message);
                        resumes[i] = data.GetApplicantResume(Data.Source.HUNTFLOW, accountId, applicant.id, applicant.external[hhIndex].id);
                        continue;
                    }
                }
                resumes[i] = null;
            }

            ApplicantStep[] steps;
            try
            {
                steps = data.GetAllApplicantSteps(Data.Source.CACHE, accountId);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                steps = data.GetAllApplicantSteps(Data.Source.HUNTFLOW, accountId);
            }

            RejectionReason[] rejectionReasons;
            try
            {
                rejectionReasons = data.GetAllRejectionReasons(Source.CACHE, accountId);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                rejectionReasons = data.GetAllRejectionReasons(Source.HUNTFLOW, accountId);
            }

            Tag[] tags;
            try
            {
                tags = data.GetAllTags(Source.CACHE, accountId);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                tags = data.GetAllTags(Source.HUNTFLOW, accountId);
            }

            Vacancy[] vacancies;
            try
            {
                vacancies = data.GetAllVacancies(Source.CACHE, accountId, false, new GetRequest.VacancyState[] { GetRequest.VacancyState.OPEN, GetRequest.VacancyState.HOLD, GetRequest.VacancyState.CLOSED });
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                vacancies = data.GetAllVacancies(Source.HUNTFLOW, accountId, false, new GetRequest.VacancyState[] { GetRequest.VacancyState.OPEN, GetRequest.VacancyState.HOLD, GetRequest.VacancyState.CLOSED });
            }

            Vacancy vacancy = null;
            foreach (var parseVacancy in vacancies)
            {
                if (parseVacancy.id == vacancyId)
                {
                    vacancy = parseVacancy;
                    break;
                }
            }

            return new ApplicantsOrderData(accountId, vacancyId, vacancy, applicants, worklogs, rejectionReasons, steps, resumes, tags);
        }
    }
}
