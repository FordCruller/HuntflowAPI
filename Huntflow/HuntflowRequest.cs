using System;
using static HuntflowAPI.GetRequest;
using static HuntflowAPI.HuntflowSurveyQuestionarySchemas;
using static HuntflowAPI.HuntflowUsers;
using static HuntflowAPI.VacancyRequestList;
using static HuntflowAPI.HuntflowApplicantFeedbackForms;
using System.Collections.Generic;

namespace HuntflowAPI
{
    internal class HuntflowRequest
    {
        private GetRequest getRequest;

        public HuntflowRequest(AuthToken token)
        {
            this.getRequest = new GetRequest(token);
        }

        public HuntflowUser GetInformationAboutCurrentUser()
        {
            return new HuntflowUser(getRequest.GetInformationAboutCurrentUser());
        }

        public HuntflowOrganizations GetAllAvailableOrganizations()
        {
            return new HuntflowOrganizations(getRequest.GetAllAvailableOrganizations());
        }

        public HuntflowOrganizationInformation GetInformationAboutOrganization(int accountId) 
        {
            return new HuntflowOrganizationInformation(getRequest.GetInformationAboutOrganization(accountId));
        }

        public HuntflowUserEmailAccountList GetUserEmailAccounts()
        {
            return new HuntflowUserEmailAccountList(getRequest.GetUserEmailAccounts());
        }

        public HuntflowUserCalendarAccountList GetUserCalendarAccounts()
        {
            return new HuntflowUserCalendarAccountList(getRequest.GetUserCalendarAccounts());
        }

        public HuntflowVacancyRequestSchemaList GetAllVacancyRequestSchemas(int accountId)
        {
            return new HuntflowVacancyRequestSchemaList(getRequest.GetAllVacancyRequestSchemas(accountId));
        }

        public HuntflowVacancyRequestSchemaList.HuntflowVacancyRequestSchema GetVacancyRequestSchema(int accountId, int accountVacancyRequestId)
        {
            return new HuntflowVacancyRequestSchemaList.HuntflowVacancyRequestSchema(getRequest.GetVacancyRequestSchema(accountId, accountVacancyRequestId));
        }

        public VacancyRequestList GetAllVacancyRequests(int accountId, int count, int page, int vacancyId, bool values)
        {
            return new VacancyRequestList(getRequest.GetAllVacancyRequests(accountId, count, page, vacancyId, values));
        }

        public HuntflowVacancyRequest GetAVacancyRequest(int accountId, int vacancyRequestId)
        {
            return new HuntflowVacancyRequest(getRequest.GetAVacancyRequest(accountId, vacancyRequestId));
        }

        public HuntflowVacancyRequestSchemaList.SchemaField GetOrganizationVacancysAdditionalFieldsSchema(int accountId)
        {
            return new HuntflowVacancyRequestSchemaList.SchemaField(getRequest.GetOrganizationVacancysAdditionalFieldsSchema(accountId));
        }

        public HuntflowVacancyList GetAllVacancies(int accountId, int count, int page, bool mine, VacancyState[] states)
        {
            return new HuntflowVacancyList(getRequest.GetAllVacancies(accountId, count, page, mine, states));
        }

        public HuntflowVacancyList.HuntflowVacancy GetAVacancy(int accountId, int vacancyId)
        {
            return new HuntflowVacancyList.HuntflowVacancy(getRequest.GetAVacancy(accountId, vacancyId));
        }

        public HuntflowVacancyLogs GetAVacancyLogs(int accountId, int vacancyId, DateTime dateBegin, DateTime dateEnd, int count, int page)
        {
            return new HuntflowVacancyLogs(getRequest.GetAVacancyLogs(accountId, vacancyId, dateBegin, dateEnd, count, page));
        }

        public HuntflowVacancyPeriod GetAVacancyPeriods(int accountId, int vacancyId, DateTime dateBegin, DateTime dateEnd)
        {
            return new HuntflowVacancyPeriod(getRequest.GetAVacancyPeriods(accountId, vacancyId, dateBegin, dateEnd));
        }

        public HuntflowVacancyFrame GetALastVacancyFrame(int accountId, int vacancyId)
        {
            return new HuntflowVacancyFrame(getRequest.GetALastVacancyFrame(accountId, vacancyId));
        }

        public HuntflowVacancyFrameList GetAListOfVacancyFrames(int accountId, int vacancyId)
        {
            return new HuntflowVacancyFrameList(getRequest.GetAListOfVacancyFrames(accountId, vacancyId));
        }

        public HuntflowVacancyQuotasInFrame GetAVacancyQuotasInFrame(int accountId, int vacancyId, int frameId)
        {
            return new HuntflowVacancyQuotasInFrame(getRequest.GetAVacancyQuotasInFrame(accountId, vacancyId, frameId));
        }

        /*Agreement's state of applicant to personal data processing.
          Available if the Personal Data module is enabled for organization.
          Cannot be supplied if the status parameter is passed.*/
        public HuntflowApplicants GetAllApplicants(int accountId, int count, int page, AgreementState agreementState)
        {
            return new HuntflowApplicants(getRequest.GetAllApplicants(accountId, count, page, agreementState));
        }

        public HuntflowApplicants GetAllApplicants(int accountId, int count, int page, int recruitmentStatusId, int vacancyId)
        {
            return new HuntflowApplicants(getRequest.GetAllApplicants(accountId, count, page, recruitmentStatusId, vacancyId));
        }

        public HuntflowApplicants GetAllApplicants(int accountId, int count, int page, int vacancyId)
        {
            return new HuntflowApplicants(getRequest.GetAllApplicants(accountId, count, page, vacancyId));
        }

        public HuntflowApplicant GetAnApplicant(int accountId, int applicantId)
        {
            return new HuntflowApplicant(getRequest.GetAnApplicant(accountId, applicantId));
        }

        public HuntflowApplicants SearchApplicants(int accountId, string q, Field field, int[] tagIds, int[] statusids, int[] rejectionReasonids, bool onlyCurrentStatus, int[] vacancyIds, int[] accountSourceIds, int count, int page)
        {
            return new HuntflowApplicants(getRequest.SearchApplicants(accountId, q, field, tagIds, statusids, rejectionReasonids, onlyCurrentStatus, vacancyIds, accountSourceIds, count, page));
        }

        public HuntflowApplicantWorklogs GetAnApplicantsWorklog(int accountId, int applicantId, LogType[] types, int count, int page, bool personal, int vacancyId)
        {
            return new HuntflowApplicantWorklogs(getRequest.GetAnApplicantsWorklog(accountId, applicantId, types, count, page, personal, vacancyId));
        }

        public HuntflowApplicantsResume GetAnApplicantsResume(int accountId, int applicantId, int externalId)
        {
            return new HuntflowApplicantsResume(getRequest.GetAnApplicantsResume(accountId, applicantId, externalId));
        }

        public HuntflowResumeSources GetAllApplicantsResumeSources(int accountId)
        {
            return new HuntflowResumeSources(getRequest.GetAllApplicantsResumeSources(accountId));
        }

        public HuntflowRecruitmentStatuses GetAllRecruitmentStatuses(int accountId)
        {
            return new HuntflowRecruitmentStatuses(getRequest.GetAllRecruitmentStatuses(accountId));
        }

        public RecruitmentStatusGroups GetAListOfRecruitmentStatusGroups(int accountId)
        {
            return new RecruitmentStatusGroups(getRequest.GetAListOfRecruitmentStatusGroups(accountId));
        }

        public HuntflowRejectionReasons GetAllRejectionReasons(int accountId)
        {
            return new HuntflowRejectionReasons(getRequest.GetAllRejectionReasons(accountId));
        }

        public HuntflowTags GetAllTags(int accountId)
        {
            return new HuntflowTags(getRequest.GetAllTags(accountId));
        }

        public HuntflowTags.HuntflowTag GetATag(int accountId, int tagId)
        {
            return new HuntflowTags.HuntflowTag(getRequest.GetATag(accountId, tagId));
        }

        public TagIds GetAllApplicantsTags(int accountId, int applicantId)
        {
            return new TagIds(getRequest.GetAllApplicantsTags(accountId, applicantId));
        }

        public HuntflowQuestionarySchema GetOrganizationApplicantsQuestionarySchema(int accountId)
        {
            return new HuntflowQuestionarySchema(getRequest.GetOrganizationApplicantsQuestionarySchema(accountId));
        }

        public string GetAnApplicantsQuestionary(int accountId)
        {
            return getRequest.GetOrganizationApplicantsQuestionarySchema(accountId);
        }

        public HuntflowOfferList GetAllOrganizationOffers(int accountId)
        {
            return new HuntflowOfferList(getRequest.GetAllOrganizationOffers(accountId));
        }

        public HuntflowOfferList GetAnOrganizationOfferWithItsSchema(int accountId, int offerId)
        {
            return new HuntflowOfferList(getRequest.GetAnOrganizationOfferWithItsSchema(accountId, offerId));
        }

        public HuntflowApplicantsVacancyOffer GetAnApplicantsVacancyOffer(int accountId, int applicantId, int vacancyFrameId, bool normalize)
        {
            return new HuntflowApplicantsVacancyOffer(getRequest.GetAnApplicantsVacancyOffer(accountId, applicantId, vacancyFrameId, normalize));
        }

        public HuntflowCompanyDivisions GetAllCompanyDivisions(int accountId, bool onlyAvailable)
        {
            return new HuntflowCompanyDivisions(getRequest.GetAllCompanyDivisions(accountId, onlyAvailable));
        }

        public HuntflowCompanyDivisions GetCompanyDivisionsAvailableToTheSpecifiedUser(int accountId, int coworkerId)
        {
            return new HuntflowCompanyDivisions(getRequest.GetCompanyDivisionsAvailableToTheSpecifiedUser(accountId, coworkerId));
        }

        public HuntflowOrganizationRegions GetAllOrganizationRegions(int accountId)
        {
            return new HuntflowOrganizationRegions(getRequest.GetAllOrganizationRegions(accountId));
        }

        public HuntflowCustomDictionaries GetAllCustomDictionaries(int accountId)
        {
            return new HuntflowCustomDictionaries(getRequest.GetAllCustomDictionaries(accountId));
        }

        public CustomDictionary GetACustomDictionary(int accountId, string dictionaryCode)
        {
            return new CustomDictionary(getRequest.GetACustomDictionary(accountId, dictionaryCode));
        }

        public HuntflowSecurityLogs GetAllSecurityLogs(int accountId, SecurityLogsType[] types, int count, int nextId, int previousId)
        {
            return new HuntflowSecurityLogs(getRequest.GetAllSecurityLogs(accountId, types, count, nextId, previousId));
        }

        public HuntflowStatusOfASystemDelayedTask GetStatusOfASystemDelayedTask(int accountId, int taskId)
        {
            return new HuntflowStatusOfASystemDelayedTask(getRequest.GetStatusOfASystemDelayedTask(accountId, taskId));
        }

        public HuntflowWebhooks GetAllWebhooks(int accountId)
        {
            return new HuntflowWebhooks(getRequest.GetAllWebhooks(accountId));
        }

        public HuntflowProductionCalendars GetAllProductionCalendars(int accountId)
        {
            return new HuntflowProductionCalendars(getRequest.GetAllWebhooks(accountId));
        }

        public ProductionCalendar GetAProductionCalendar(int accountId)
        {
            return new ProductionCalendar(getRequest.GetAProductionCalendar(accountId));
        }

        public HuntflowNonWorkingDays GetNonWorkingDaysInAGivenPeriod(int calendarId, DateTime deadline, DateTime start, bool verbode)
        {
            return new HuntflowNonWorkingDays(getRequest.GetNonWorkingDaysInAGivenPeriod(calendarId, deadline, start, verbode));
        }

        public HuntflowOrganizationsProductionCalendar GetOrganizationsProductionCalendar(int accountId)
        {
            return new HuntflowOrganizationsProductionCalendar(getRequest.GetOrganizationsProductionCalendar(accountId));
        }

        public HuntflowCoworkers GetAllCoworkers(int accountId, UserType[] types, bool fetchPermissions, int[] vacancyIds, int count, int page)
        {
            return new HuntflowCoworkers(getRequest.GetAllCoworkers(accountId, types, fetchPermissions, vacancyIds, count, page));
        }

        public HuntflowCoworker GetACoworker(int accountId, int coworkerId, int vacancyId)
        {
            return new HuntflowCoworker(getRequest.GetACoworker(accountId, coworkerId, vacancyId));
        }

        public HuntflowEmailTemplates GetAllEmailTemplates(int accountId, bool editable)
        {
            return new HuntflowEmailTemplates(getRequest.GetAllEmailTemplates(accountId, editable));
        }

        public HuntflowVacancyReasons GetAllVacancyCloseReasons(int accountId)
        {
            return new HuntflowVacancyReasons(getRequest.GetAllVacancyCloseReasons(accountId));
        }

        public HuntflowVacancyReasons GetAllVacancyHoldReasons(int accountId)
        {
            return new HuntflowVacancyReasons(getRequest.GetAllVacancyHoldReasons(accountId));
        }

        public HuntflowApplicantSurveyForm GetAnApplicantSurveyForm(int accountId, int surveyId)
        {
            return new HuntflowApplicantSurveyForm(getRequest.GetAnApplicantSurveyForm(accountId, surveyId));
        }

        public HuntflowUser GetAUser(int accountId, int userId)
        {
            return new HuntflowUser(getRequest.GetAUser(accountId, userId));
        }

        public HuntflowUsers GetAListOfAllUsersWithTheirForeignIdentifiers(int accountId, int count, int page)
        {
            return new HuntflowUsers(getRequest.GetAListOfAllUsersWithTheirForeignIdentifiers(accountId, count, page));
        }

        public int GetInternalIDOfAnExistingUserByHisForeignIdentifier(int accountId, int foreignUserId)
        {
            string json = getRequest.GetInternalIDOfAnExistingUserByHisForeignIdentifier(accountId, foreignUserId);
            return int.Parse(json.Split(':')[1].Trim());
        }
        public HuntflowUser GetAnExistingUserByHisForeignIdentifier(int accountId, int foreignUserId)
        {
            return new HuntflowUser(getRequest.GetAnExistingUserByHisForeignIdentifier(accountId, foreignUserId));
        }

        public HuntflowControlTaskResult GetForeignUserControlTaskResult(int accountId, string taskId)
        {
            return new HuntflowControlTaskResult(getRequest.GetForeignUserControlTaskResult(accountId, taskId));
        }

        public HuntflowApplicantFeedbackForms GetAllApplicantFeedbackForms(int accountId, bool active)
        {
            return new HuntflowApplicantFeedbackForms(getRequest.GetAllApplicantFeedbackForms(accountId, active));
        }

        public HuntflowApplicantFeedbackForm GetAnApplicantFeedbackForm(int accountId, int surveyId)
        {
            return new HuntflowApplicantFeedbackForm(getRequest.GetAnApplicantFeedbackForm(accountId, surveyId));
        }

        public HuntflowSurveyQuestionarySchemas GetAllSurveyQuestionarySchemasForApplicants(int accountId, bool active)
        {
            return new HuntflowSurveyQuestionarySchemas(getRequest.GetAllSurveyQuestionarySchemasForApplicants(accountId, active));
        }

        public SurveySchema GetSurveyQuestionarySchemaForApplicants(int accountId, int surveyId)
        {
            return new SurveySchema(getRequest.GetSurveyQuestionarySchemaForApplicants(accountId, surveyId));
        }

        public SurveySchema GetSurveyQuestionaryForApplicantByID(int accountId, int questionaryId)
        {
            return new SurveySchema(getRequest.GetSurveyQuestionaryForApplicantByID(accountId, questionaryId));
        }

        public SurveySchema GetSurveyQuestionaryAnswerByID(int accountId, int answerId)
        {
            return new SurveySchema(getRequest.GetSurveyQuestionaryAnswerByID(accountId, answerId));
        }
    }
}
