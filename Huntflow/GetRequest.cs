using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using static HuntflowAPI.HuntflowApplicantWorklogs;
using System.Collections.Generic;

namespace HuntflowAPI
{
    public class GetRequest
    {
        private AuthToken token;

        private readonly string apiUrl = "https://api.huntflow.ru/v2";
        private readonly string authorizationToken = $"Bearer ";

        public GetRequest(AuthToken token)
        {
            this.token = token ?? throw new ArgumentNullException(nameof(token));
            authorizationToken += token.access_token;
        }

        //Returns information about the user associated with the passed authentication
        public string GetInformationAboutCurrentUser()
        {
            return SendRequest(apiUrl + "/me", null).Result;
        }

        //Returns a list of available organizations for the user associated with the passed authentication
        public string GetAllAvailableOrganizations()
        {
            return SendRequest(apiUrl + "/accounts", null).Result;
        }

        //Returns information about specified organization
        public string GetInformationAboutOrganization(int accountId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}", null).Result;
        }

        //Returns a list of user email accounts
        public string GetUserEmailAccounts()
        {
            return SendRequest(apiUrl + "/email_accounts", null).Result;
        }

        //Returns a list of user calendar accounts with associated calendars
        public string GetUserCalendarAccounts()
        {
            return SendRequest(apiUrl + "/calendar_accounts", null).Result;
        }

        //Returns a list of vacancy requests schemas available in the organization
        public string GetAllVacancyRequestSchemas(int accountId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/account_vacancy_requests", null).Result;
        }

        //Returns the specified vacancy request schema
        public string GetVacancyRequestSchema(int accountId, int accountVacancyRequestId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/account_vacancy_requests/{accountVacancyRequestId}", null).Result;
        }

        //Returns a list of vacancy requests
        public string GetAllVacancyRequests(int accountId, int count, int page, int vacancyId, bool values)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/vacancy_requests", $"?count={count}&page={page}&vacancy_id={vacancyId}&values={values}").Result;
        }

        //Returns a vacancy requests
        public string GetAVacancyRequest(int accountId, int vacancyRequestId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/vacancy_requests/{vacancyRequestId}", null).Result;
        }

        //Returns a schema of additional fields for vacancies set in organization
        public string GetOrganizationVacancysAdditionalFieldsSchema(int accountId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/vacancies/additional_fields", null).Result;
        }

        //Returns a list of vacancies
        public string GetAllVacancies(int accountId, int count, int page, bool mine, VacancyState[] states)
        {
            string statesString = "";
            foreach (var state in states)
            {
                statesString += $"state={state}&";
            }
            statesString = statesString.Remove(statesString.Length - 1);

            return SendRequest(apiUrl + $"/accounts/{accountId}/vacancies", $"?count={count}&page={page}&mine={mine.ToString().ToLower()}&{statesString}").Result; 
        }

        //Returns a vacancy
        public string GetAVacancy(int accountId, int vacancyId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/vacancies/{vacancyId}", null).Result;
        }

        //Returns a list of vacancy logs with pagination
        public string GetAVacancyLogs(int accountId, int vacancyId, DateTime dateBegin, DateTime dateEnd, int count, int page)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/vacancies/{vacancyId}/logs", $"?date_begin={dateBegin.ToString("yyyy-MM-ddTHH%3Amm%3Asszzz")}0&date_end={dateEnd.ToString("yyyy-MM-ddTHH%3Amm%3Asszzz")}&count={count}&page={page}").Result;
        }

        //Returns the periods of the vacancy
        public string GetAVacancyPeriods(int accountId, int vacancyId, DateTime dateBegin, DateTime dateEnd)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/vacancies/{vacancyId}/periods", $"?date_begin={dateBegin.ToString("yyyy-MM-ddTHH%3Amm%3Asszzz")}0&date_end={dateEnd.ToString("yyyy-MM-ddTHH%3Amm%3Asszzz")}").Result;
        }

        //Returns the last frame of a vacancy
        public string GetALastVacancyFrame(int accountId, int vacancyId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/vacancies/{vacancyId}/frame", null).Result;
        }

        //Returns the last frame of a vacancy
        public string GetAListOfVacancyFrames(int accountId, int vacancyId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/vacancies/{vacancyId}/frames", null).Result;
        }

        /* Get a vacancy quotas
        
         * A vacancy can have one or more fill quotas. Fill quotas bind vacancies, vacancy requests (optional),
         * a number of applicants to hire and deadlines to close vacancies.
         * Selectable number of fill quotas for one vacancy allows to precise control times of vacancy requests closing.
         * And at the same time there may be several vacancy requests on one vacancy,
         * also requests may be attached to the vacancy at any moment.
         * For a vacancy it's allowed to have one fill quota without a vacancy request.*/
        public string GetAVacancyQuotas(int accountId, int vacancyId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/vacancies/{vacancyId}/quotas", null).Result;
        }

        //Returns quotas for vacancy frame
        public string GetAVacancyQuotasInFrame(int accountId, int vacancyId, int frameId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/vacancies/{vacancyId}/frames/{frameId}/quotas", null).Result;
        }

        //Returns a list of applicants with pagination. This is a simple method that has limited filtering options. Use search method for more precise filtering
        /*Agreement's state of applicant to personal data processing.
          Available if the Personal Data module is enabled for organization.
          Cannot be supplied if the status parameter is passed.*/
        public string GetAllApplicants(int accountId, int count, int page, AgreementState agreementState)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/applicants", $"?count={count}&page={page}&agreement_state={agreementState}").Result;
        }
        public string GetAllApplicants(int accountId, int count, int page, int recruitmentStatusId, int vacancyId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/applicants", $"?count={count}&page={page}&status={recruitmentStatusId}&vacancy={vacancyId}").Result;
        }

        public string GetAllApplicants(int accountId, int count, int page, int vacancyId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/applicants", $"?count={count}&page={page}&vacancy={vacancyId}").Result;
        }

        //Returns the specified applicant
        public string GetAnApplicant(int accountId, int applicantId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/applicants/{applicantId}", null).Result;
        }

        //Returns a list of found applicants. Limited by 20k items. If you want to get more, use applicants search by cursor
        public string SearchApplicants(int accountId, string q, Field field, int[] tagIds, int[] statusids, int[] rejectionReasonids, bool onlyCurrentStatus, int[] vacancyIds, int[] accountSourceIds, int count, int page)
        {
            string tagsString = "";
            foreach (var tag in tagIds)
            {
                tagsString += $"tag={tag}&";
            }
            string statusesString = "";
            foreach (var status in statusids)
            {
                statusesString += $"status={status}&";
            }
            string rejectionReasonsString = "";
            foreach (var rejectionReason in rejectionReasonids)
            {
                rejectionReasonsString += $"rejection_reason={rejectionReason}&";
            }
            string vacanciesString = "";
            foreach (var vacancy in vacancyIds)
            {
                vacanciesString += $"vacancy={vacancy}&";
            }
            string accountSourceString = "";
            foreach (var accountSource in accountSourceIds)
            {
                accountSourceString += $"account_source={accountSource}&";
            }

            return SendRequest(apiUrl + $"/accounts/{accountId}/applicants/search", $"?q={q}&field={field}&{tagsString}{statusesString}{rejectionReasonsString}only_current_status={onlyCurrentStatus}&{vacanciesString}{accountSourceString}count={count}&page={page}").Result;
        }

        //Returns a list of found applicants and a cursor to the next page. To get the next page, you have to copy next_page_cursor from the answer and pass it in the request params
        public string SearchApplicantsByCursor(int accountId, string q, string nextPageCursor, Field field, int[] tagIds, int[] statusids, int[] rejectionReasonids, bool onlyCurrentStatus, int[] vacancyIds, int[] accountSourceIds)
        {
            string tagsString = "";
            foreach (var tag in tagIds)
            {
                tagsString += $"tag={tag}&";
            }
            string statusesString = "";
            foreach (var status in statusids)
            {
                statusesString += $"status={status}&";
            }
            string rejectionReasonsString = "";
            foreach (var rejectionReason in rejectionReasonids)
            {
                rejectionReasonsString += $"rejection_reason={rejectionReason}&";
            }
            string vacanciesString = "";
            foreach (var vacancy in vacancyIds)
            {
                vacanciesString += $"vacancy={vacancy}&";
            }
            string accountSourceString = "";
            foreach (var accountSource in accountSourceIds)
            {
                accountSourceString += $"account_source={accountSource}&";
            }
            accountSourceString.Remove(accountSourceIds.Length - 1);

            return SendRequest(apiUrl + $"/accounts/{accountId}/applicants/search_by_cursor", $"?q={q}&field={field}&{tagsString}{statusesString}{rejectionReasonsString}only_current_status={onlyCurrentStatus}&{vacanciesString}{accountSourceString}").Result;
        }

        //Returns an applicant resume
        public string GetAnApplicantsWorklog(int accountId, int applicantId, LogType[] types, int count, int page, bool personal, int vacancyId)
        {
            string typesString = "";
            foreach (var type in types)
            {
                typesString += $"type={type}&";
            }

            return SendRequest(apiUrl + $"/accounts/{accountId}/applicants/{applicantId}/logs", $"?{typesString}count={count}&page={page}&personal={personal.ToString().ToLower()}&vacancy={vacancyId}").Result;
        }

        //Returns an applicant resume
        public string GetAnApplicantsResume(int accountId, int applicantId, int externalId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/applicants/{applicantId}/externals/{externalId}", null).Result;
        }

        //Returns a list of applicant's resume sources
        public string GetAllApplicantsResumeSources(int accountId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/applicants/sources", null).Result;
        }

        //Returns a list of recruitment statuses (hiring stages)
        public string GetAllRecruitmentStatuses(int accountId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/vacancies/statuses", null).Result;
        }

        //Returns a list of recruitment status groups
        public string GetAListOfRecruitmentStatusGroups(int accountId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/vacancies/status_groups", null).Result;
        }

        //Returns a list of applicant on vacancy rejection reasons
        public string GetAllRejectionReasons(int accountId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/rejection_reasons", null).Result;
        }

        //Returns a list of tags in the organization
        public string GetAllTags(int accountId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/tags", null).Result;
        }

        //Returns the specified tag
        public string GetATag(int accountId, int tagId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/tags/{tagId}", null).Result;
        }

        //Returns a list of applicant's tags IDs
        public string GetAllApplicantsTags(int accountId, int applicantId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/applicants/{applicantId}/tags", null).Result;
        }

        //Returns a questionary for the specified applicant
        public string GetAnApplicantsQuestionary(int accountId, int applicantId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/applicants/{applicantId}/questionary", null).Result;
        }

        //Returns a schema of applicant's questionary for organization
        public string GetOrganizationApplicantsQuestionarySchema(int accountId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/applicants/questionary", null).Result;
        }

        //Returns a list of organization's offers
        public string GetAllOrganizationOffers(int accountId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/offers", null).Result;
        }

        //Returns an organization's offer with a schema of values
        public string GetAnOrganizationOfferWithItsSchema(int accountId, int offerId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/offers/{offerId}", null).Result;
        }

        //Returns an applicant offer in PDF format
        public string GetAnApplicantsPDFOffer(int accountId, int applicantId, int offerId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/applicants/{applicantId}/offers/{offerId}/pdf", null).Result;
        }

        /* Returns an applicant's offer for vacancy with its values.
         * The composition of the values depends on the organization's offer settings*/
        public string GetAnApplicantsVacancyOffer(int accountId, int applicantId, int vacancyFrameId, bool normalize)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/applicants/{applicantId}/vacancy_frames/{vacancyFrameId}/offer", $"?normalize={normalize}").Result;
        }

        //Returns a list of company divisions
        public string GetAllCompanyDivisions(int accountId, bool onlyAvailable)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/divisions", $"?only_available={onlyAvailable}").Result;
        }

        /* Returns a list of company divisions available to the specified user.
         * Note: Coworker's company divisions are cached for 10 minutes. 
         * If a coworker's company divisions list has been changed, 
         * these changes will be displayed after a maximum of 10 minutes*/
        public string GetCompanyDivisionsAvailableToTheSpecifiedUser(int accountId, int coworkerId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/coworkers/{coworkerId}/divisions", null).Result;
        }

        //Returns a list of organization regions
        public string GetAllOrganizationRegions(int accountId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/regions", null).Result;
        }

        ///
        //Returns a list of organization's custom dictionaries
        public string GetAllCustomDictionaries(int accountId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/dictionaries", null).Result;
        }

        public string GetACustomDictionary(int accountId, string dictionaryCode)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/dictionaries/{dictionaryCode}", null).Result;
        }

        /* Returns a list of security logs sorted in descending order (from newest to older)
         * Pagination is implemented using the next_id and previous_id parameters.
         * If the response body contains the next_id field, and you need to get older logs, supply its value as query-parameter.
         * To get logs that are newer than a particular log, you need to supply this particular log ID as previous_id query-parameter*/
        public string GetAllSecurityLogs(int accountId, SecurityLogsType[] types, int count, int nextId, int previousId)
        {
            string typesString = "";
            foreach (var type in types)
            {
                typesString += $"type={type}&";
            }

            return SendRequest(apiUrl + $"/accounts/{accountId}/action_logs", $"?type={typesString}count={count}&next_id={nextId}&previous_id={previousId}").Result;
        }

        public string GetStatusOfASystemDelayedTask(int accountId, int taskId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/delayed_tasks/{taskId}", null).Result;
        }

        public string GetAllWebhooks(int accountId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/hooks", null).Result;
        }

        //Returns a list of production calendars
        public string GetAllProductionCalendars(int accountId)
        {
            return SendRequest(apiUrl + $"/production_calendars", null).Result;
        }

        //Returns a production calendar
        public string GetAProductionCalendar(int calendarId)
        {
            return SendRequest(apiUrl + $"/production_calendars/{calendarId}", null).Result;
        }

        //Returns the total number of non-working\working days and a list of weekends and holidays within a range
        public string GetNonWorkingDaysInAGivenPeriod(int calendarId, DateTime deadline, DateTime start, bool verbode)
        {
            return SendRequest(apiUrl + $"/production_calendars/{calendarId}/days/{deadline.ToString("yyyy-MM-dd")}", $"?start={start.ToString("yyyy-MM-dd")}&verbose={verbode}").Result;
        }

        //Returns a deadline after {days} working days
        public string GetADeadlineDateEvaluationTakingIntoAccountTheNonWorkingDays(int calendarId, int days, DateTime start)
        {
            return SendRequest(apiUrl + $"/production_calendars/{calendarId}/deadline/{days}", $"?start={start.ToString("yyyy-MM-dd")}").Result;
        }

        //Returns a date in {days} working days ahead, according to {calendar_id} production calendar
        public string GetAStartDateEvaluationTakingIntoAccountTheNonWorkingDays(int calendarId, int days, DateTime deadline)
        {
            return SendRequest(apiUrl + $"/production_calendars/{calendarId}/start/{days}", $"?deadline={deadline.ToString("yyyy-MM-dd")}").Result;
        }

        //Returns a date in {days} working days ahead, according to {calendar_id} production calendar
        public string GetOrganizationsProductionCalendar(int accountId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/calendar", null).Result;
        }


        /* Returns a list of coworkers with pagination
         * Restrictions:
         * Users of type watcher can only see coworkers who are on the same vacancies with them. Coworker permissions are not available for users of this type.*/
        public string GetAllCoworkers(int accountId, UserType[] types, bool fetchPermissions, int[] vacancyIds, int count, int page)
        {
            string typesString = "";
            foreach (var type in types)
            {
                typesString += $"type={type}&";
            }

            string vacancyIdsString = "";
            foreach (var id in vacancyIds)
            {
                vacancyIdsString += $"type={id}&";
            }

            return SendRequest(apiUrl + $"/accounts/{accountId}/coworkers", $"?type={typesString}fetch_permissions={fetchPermissions}&vacancy_id={vacancyIdsString}count={count}&page={page}").Result;
        }

        //Returns the specified coworker with a list of their permissions
        public string GetACoworker(int accountId, int coworkerId, int vacancyId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/coworkers/{coworkerId}", $"?vacancy_id={vacancyId}").Result;
        }

        /* Returns a list of email templates.
         * Restrictions:
         * Users of type watcher do not have access to the list of email templates*/
        public string GetAllEmailTemplates(int accountId, bool editable)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/mail/templates", $"?editable={editable}").Result;
        }

        //Returns a list of vacancy close reasons
        public string GetAllVacancyCloseReasons(int accountId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/vacancy_close_reasons", null).Result;
        }

        //Returns a list of vacancy hold reasons
        public string GetAllVacancyHoldReasons(int accountId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/vacancy_hold_reasons", null).Result;
        }

        public string GetAnApplicantSurveyForm(int accountId, int surveyId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/surveys/type_a/{surveyId}", null).Result;
        }

        /* Returns the specified user with a list of his permissions
         * Restrictions: Users of type watcher cannot see other users detail information.*/
        public string GetAUser(int accountId, int userId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/users/{userId}", null).Result;
        }

        /* Returns all users in organization with their permissions (vacancies permissions are not included), available divisions and their manager's identifiers. 
         * All identifiers in response are foreign.
         * Restrictions: Divisions and managers fields available when this service kinds are active.
         * null in permissions or/and divisions fields means "all"*/
        public string GetAListOfAllUsersWithTheirForeignIdentifiers(int accountId, int count, int page)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/users/foreign", $"?count={count}&page={page}").Result;
        }

        /* Returns internal ID of the specified user. Request user's identifier is foreign, response user's identifier is internal
         * Example foreignUserId: "user-032155"*/
        public string GetInternalIDOfAnExistingUserByHisForeignIdentifier(int accountId, int foreignUserId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/users/foreign/{foreignUserId}/id", null).Result;
        }

        /*Returns the specified user with his permissions, available divisions and his manager's identificators. 
         * All identifiers in request and response are foreign
         * Restrictions: Divisions and managers fields available when these service kinds are active.
         * null in permissions or/and divisions fields means "all".*/
        public string GetAnExistingUserByHisForeignIdentifier(int accountId, int foreignUserId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/users/foreign/{foreignUserId}", null).Result;
        }

        //Returns users management task handling result. All user identifiers in response are foreign
        public string GetForeignUserControlTaskResult(int accountId, string taskId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/users/foreign/task/{taskId}", null).Result;
        }

        //Returns all applicant feedback forms in organization
        public string GetAllApplicantFeedbackForms(int accountId, bool active)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/surveys/type_a", $"?active={active}").Result;
        }

        //Returns given applicant feedback form
        public string GetAnApplicantFeedbackForm(int accountId, int surveyId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/surveys/type_a/{surveyId}", null).Result;
        }

        //Returns all survey questionary schemas for applicants for organization
        public string GetAllSurveyQuestionarySchemasForApplicants(int accountId, bool active)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/surveys/type_q", $"?active={active}").Result;
        }

        //Returns survey questionary schema for applicants
        public string GetSurveyQuestionarySchemaForApplicants(int accountId, int surveyId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/surveys/type_q/{surveyId}", null).Result;
        }

        //Returns survey questionary
        public string GetSurveyQuestionaryForApplicantByID(int accountId, int questionaryId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/surveys/type_q/questionaries/{questionaryId}", null).Result;
        }

        //Returns survey questionary answer. Each key in data represents a question in the survey form. Schema properties can be used to find information about the question by key
        public string GetSurveyQuestionaryAnswerByID(int accountId, int answerId)
        {
            return SendRequest(apiUrl + $"/accounts/{accountId}/surveys/type_q/answers/{answerId}", null).Result;
        }

        public async Task<string> SendRequest(string url, string queryString)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorizationToken);

            if (queryString != null)
            {
                queryString = queryString.Replace("?", "");
                string[] pathParameters = queryString.Split('&');
                queryString = "?";
                foreach (string parameter in pathParameters)
                {
                    string[] keyAndValue = parameter.Split('=');
                    if (keyAndValue.Length == 2 && !keyAndValue[0].Equals("null") && !keyAndValue[0].Equals(""))
                    {
                        queryString += $"{parameter}&";
                    }
                }
                queryString = queryString.Remove(queryString.Length - 1);
            }

            url = url + queryString;
            Console.WriteLine($"Запрос Huntflow: {url}");

            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get
            };

            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Headers.Add("Authorization", authorizationToken);
            request.Headers.Add("Connection", "keep-alive");
            request.Headers.Add("Host", "api.huntflow.ru");
            request.Headers.Add("Referer", "https://api.huntflow.ru/v2/docs");

            HttpResponseMessage response = client.SendAsync(request).Result;

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return jsonResponse;
            }
            else
            {
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
            }
        }
        
        public enum Field
        {
            all,
            education,
            experience,
            position
        }

        public enum AgreementState
        {
            not_sent,
            sent,
            accepted,
            declined,
            send_error
        }

        public enum VacancyState
        {
            OPEN,
            CLOSED,
            HOLD
        }

        public enum LogType
        {
            ADD,
            UPDATE,
            [EnumMember(Value = "VACANCY-ADD")]
            VACANCY_ADD,
            STATUS,
            COMMENT,
            DOUBLE,
            AGREEMENT,
            MAIL,
            RESPONSE
        }

        public enum FieldType
        {
            String,
            Integer,
            Text,
            Date,
            Select,
            Complex,
            Contract,
            Reason,
            Stoplist,
            Compensation,
            Dictionary,
            Income,
            PositionStatus,
            Division,
            Region,
            Url,
            Hidden,
            Html
        }

        public enum SecurityLogsType
        {
            SUCCESS_LOGIN,
            FAILED_LOGIN,
            LOGOUT,
            INVITE_ACCEPTED,
            NEW_AUTH_IN_ACCOUNT,
            VACANCY_EXTERNAL,
            ACCOUNT_MEMBER,
            DOWNLOAD_APPLICANTS
        }

        public enum UserType
        {
            owner,
            manager,
            watcher
        }
    }
}
