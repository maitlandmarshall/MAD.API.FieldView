using ConfigurationServicesEndpoint;
using FormsServicesEndpoint;
using MAD.API.FieldView.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjectServicesEndpoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace MAD.API.FieldView
{
    public class FieldViewClient
    {
        private delegate Task<string> RunApiWithPaginationCallback(int? take, int startRow);

        private const int PageSize = 500;

        private readonly string? apiToken;
        private readonly string? loginName;
        private readonly string? password;

        public FieldViewClient (string apiKey)
        {
            this.apiToken = apiKey;
        }

        public FieldViewClient (string loginName, string password)
        {
            this.loginName = loginName;
            this.password = password;
        }

        private void SetBindingTimeouts(Binding binding)
        {
            binding.CloseTimeout = TimeSpan.FromMinutes(60);
            binding.OpenTimeout = TimeSpan.FromMinutes(60);
            binding.ReceiveTimeout = TimeSpan.FromMinutes(60);
            binding.SendTimeout = TimeSpan.FromMinutes(60);
        }

        private async Task<API_ProjectServicesSoapClient> GetProjectServicesClient()
        {
            API_ProjectServicesSoapClient result = new API_ProjectServicesSoapClient(API_ProjectServicesSoapClient.EndpointConfiguration.API_ProjectServicesSoap);

            this.SetBindingTimeouts(result.Endpoint.Binding);

            return result;
        }

        private async Task<API_FormsServicesSoapClient> GetFormsServicesClient()
        {
            var result = new API_FormsServicesSoapClient(API_FormsServicesSoapClient.EndpointConfiguration.API_FormsServicesSoap);

            this.SetBindingTimeouts(result.Endpoint.Binding);

            return result;
        }

        private async Task<API_ConfigurationServicesSoapClient> GetConfigurationServicesClient()
        {
            var result = new API_ConfigurationServicesSoapClient(API_ConfigurationServicesSoapClient.EndpointConfiguration.API_ConfigurationServicesSoap);

            this.SetBindingTimeouts(result.Endpoint.Binding);

            return result;
        }

        private ProjectServicesEndpoint.ArrayOfInt GetProjectArrayOfInt(int[] array)
        {
            ProjectServicesEndpoint.ArrayOfInt arrayOfInt;

            if (array != null)
            {
                arrayOfInt = new ProjectServicesEndpoint.ArrayOfInt();
                arrayOfInt.AddRange(array ?? new int[] { });
            }
            else
            {
                arrayOfInt = null;
            }

            return arrayOfInt;
        }

        private ConfigurationServicesEndpoint.ArrayOfInt GetConfigArrayOfInt(int[] array)
        {
            ConfigurationServicesEndpoint.ArrayOfInt arrayOfInt;

            if (array != null)
            {
                arrayOfInt = new ConfigurationServicesEndpoint.ArrayOfInt();
                arrayOfInt.AddRange(array ?? new int[] { });
            }
            else
            {
                arrayOfInt = null;
            }

            return arrayOfInt;
        }

        private FormsServicesEndpoint.ArrayOfInt GetFormArrayOfInt(int[] array)
        {
            FormsServicesEndpoint.ArrayOfInt arrayOfInt;

            if (array != null)
            {
                arrayOfInt = new FormsServicesEndpoint.ArrayOfInt();
                arrayOfInt.AddRange(array ?? new int[] { });
            }
            else
            {
                arrayOfInt = null;
            }

            return arrayOfInt;
        }

        private async Task<IEnumerable<TEntity>> RunApiWithPagination<TEntity>(RunApiWithPaginationCallback apiCall, int? take = null, int? startRow = null)
        {
            List<TEntity> finalResult = new List<TEntity>();
            IEnumerable<TEntity> pageResult;

            int currentRow = startRow ?? 1;

            do
            {
                pageResult = this.DeserializeResponse<TEntity>(await apiCall(take, currentRow));
                finalResult.AddRange(pageResult);

                currentRow = finalResult.Count() + 1;

            } while (pageResult.Count() == PageSize || take.HasValue && finalResult.Count >= take);

            if (take.HasValue)
            {
                return finalResult.Take(take.Value).ToList();
            }
            else
            {
                return finalResult;
            }
        }

        private IEnumerable<TEntity> DeserializeResponse <TEntity>(string json)
        {
            FieldViewResponse<TEntity> response = new FieldViewResponseFactory().Create<TEntity>(json);

            if (response.Status.Code != 2)
                throw new FieldViewResponseException(response);

            return response.Entities;
        }

        public async Task<IEnumerable<ProjectInformation>> GetProjectDetails (string projectName = null,
                                                                                    int[] businessUnitIds = null,
                                                                                    bool? activeOnly = null,
                                                                                    int? take = null,
                                                                                    int? startRow = null)
        {
            API_ConfigurationServicesSoapClient configServicesClient = await this.GetConfigurationServicesClient();

            return await this.RunApiWithPagination<ProjectInformation>(
                apiCall: async (take, startRow) =>
                {
                    GetProjectsResponse response = await configServicesClient.GetProjectsAsync(this.apiToken, null, this.GetConfigArrayOfInt(businessUnitIds), false, 1, 500);
                    return response.Body.GetProjectsResult;
                },
                take: take,
                startRow: startRow
             );
        }

        public async Task<IEnumerable<ProjectFormTemplateInformation>> GetProjectFormTemplates(int projectId,
                                                                                               bool viewAllOrganisationsFormTemplates,
                                                                                               bool includeInactive = true)
        {
            API_FormsServicesSoapClient formsServicesClient = await this.GetFormsServicesClient();
            GetProjectFormTemplatesResponse getProjectFormTemplatesResponse = await formsServicesClient.GetProjectFormTemplatesAsync(this.apiToken, projectId, viewAllOrganisationsFormTemplates, includeInactive);

            IEnumerable<ProjectFormTemplateInformation> result = this.DeserializeResponse<ProjectFormTemplateInformation>(getProjectFormTemplatesResponse.Body.GetProjectFormTemplatesResult);

            foreach (var r in result)
                r.ProjectId = projectId;

            return result;
        }

        public async Task<IEnumerable<ProjectFormsListInformation>> GetProjectFormsList(int projectId,
                                                                                        int[] formTemplateLinkIds,
                                                                                        bool includeDeleted = true,
                                                                                        DateTime? createdDateFrom = null,
                                                                                        DateTime? createdDateTo = null,
                                                                                        DateTime? statusChangedDateFrom = null,
                                                                                        DateTime? statusChangedDateTo = null,
                                                                                        DateTime? lastmodifiedDateFrom = null,
                                                                                        DateTime? lastmodifiedDateTo = null,
                                                                                        DateTime? lastmodifiedOnServerDateFrom = null,
                                                                                        DateTime? lastmodifiedOnServerDateTo = null)
        {
            API_FormsServicesSoapClient formsServicesClient = await this.GetFormsServicesClient();
            GetProjectFormsListResponse projectFormsListResponse = await formsServicesClient.GetProjectFormsListAsync(this.apiToken, projectId, this.GetFormArrayOfInt(formTemplateLinkIds), includeDeleted, createdDateFrom, createdDateTo, statusChangedDateFrom, statusChangedDateTo, lastmodifiedDateFrom, lastmodifiedDateTo, lastmodifiedOnServerDateFrom, lastmodifiedOnServerDateTo);

            return this.DeserializeResponse<ProjectFormsListInformation>(projectFormsListResponse.Body.GetProjectFormsListResult);
        }

        public async Task<IEnumerable<ProjectFormsListUpdatedInformation>> GetProjectFormsListUpdated(int projectId,
                                                                                                      int[] formTemplateLinkIds,
                                                                                                      DateTime? lastmodifiedDateFrom = null,
                                                                                                      DateTime? lastmodifiedDateTo = null,
                                                                                                      DateTime? answerLastModifiedOnServerFrom = null,
                                                                                                      DateTime? answerLastModifiedOnServerTo = null)
        {
            API_FormsServicesSoapClient formsServicesClient = await this.GetFormsServicesClient();
            GetProjectFormsListUpdatedResponse projectFormsListUpdatedResponse = await formsServicesClient.GetProjectFormsListUpdatedAsync(this.apiToken, projectId, this.GetFormArrayOfInt(formTemplateLinkIds), lastmodifiedDateFrom, lastmodifiedDateTo, answerLastModifiedOnServerFrom, answerLastModifiedOnServerTo);

            return this.DeserializeResponse<ProjectFormsListUpdatedInformation>(projectFormsListUpdatedResponse.Body.GetProjectFormsListUpdatedResult);
        }

        public async Task<IEnumerable<FormInformation>> GetForm(string formId)
        {
            API_FormsServicesSoapClient formsServicesClient = await this.GetFormsServicesClient();
            GetFormResponse getFormResponse = await formsServicesClient.GetFormAsync(this.apiToken, formId);

            IEnumerable<FormInformation> result = this.DeserializeResponse<FormInformation>(getFormResponse.Body.GetFormResult);

            foreach (FormInformation f in result)
                f.FormId = formId;

            return result;
        }

        public async Task<IEnumerable<FormAnswerAuditTrail>> GetFormAnswerAuditTrail(string formId, int formTemplateId, string formAnswerId, bool isInTableRow = false)
        {
            if (string.IsNullOrEmpty(formId))
                throw new ArgumentException("But be not null and not empty.", nameof(formId));

            if (string.IsNullOrEmpty(formAnswerId))
                throw new ArgumentException("But be not null and not empty.", nameof(formAnswerId));

            API_FormsServicesSoapClient formsServicesClient = await this.GetFormsServicesClient();
            GetFormAnswerAuditTrailResponse response = await formsServicesClient.GetFormAnswerAuditTrailAsync(this.apiToken, formId, formTemplateId, formAnswerId, isInTableRow);

            return this.DeserializeResponse<FormAnswerAuditTrail>(response.Body.GetFormAnswerAuditTrailResult);
        }

        public async Task<IEnumerable<CommentInformation>> GetFormAnswerComments(string formAnswerId)
        {
            if (string.IsNullOrEmpty(formAnswerId))
                throw new ArgumentException("But be not null and not empty.", nameof(formAnswerId));

            API_FormsServicesSoapClient formsServicesClient = await this.GetFormsServicesClient();
            GetFormAnswerCommentsResponse response = await formsServicesClient.GetFormAnswerCommentsAsync(this.apiToken, formAnswerId);

            IEnumerable<CommentInformation> comments = this.DeserializeResponse<CommentInformation>(response.Body.GetFormAnswerCommentsResult);

            foreach (CommentInformation c in comments)
            {
                c.FormAnswerId = formAnswerId;
            }

            return comments;
        }

        public async Task<IEnumerable<FormAuditTrail>> GetFormAuditTrail(string formId)
        {
            API_FormsServicesSoapClient formsServicesClient = await this.GetFormsServicesClient();
            GetFormAuditTrailResponse response = await formsServicesClient.GetFormAuditTrailAsync(this.apiToken, formId);

            IEnumerable<FormAuditTrail> auditTrails = this.DeserializeResponse<FormAuditTrail>(response.Body.GetFormAuditTrailResult);

            foreach (FormAuditTrail a in auditTrails)
                a.FormId = formId;

            return auditTrails;
        }

        public async Task<IEnumerable<CommentInformation>> GetFormComments(string formId)
        {
            API_FormsServicesSoapClient formsServicesClient = await this.GetFormsServicesClient();
            GetFormCommentsResponse response = await formsServicesClient.GetFormCommentsAsync(this.apiToken, formId);

            IEnumerable<CommentInformation> comments = this.DeserializeResponse<CommentInformation>(response.Body.GetFormCommentsResult);

            foreach (CommentInformation c in comments)
            {
                c.FormId = formId;
            }

            return comments;
        }

        public async Task<IEnumerable<FormWorkflowStatus>> GetFormWorkflowStatusList(int formTemplateId, int? projectId = null)
        {
            API_FormsServicesSoapClient formsServicesClient = await this.GetFormsServicesClient();
            GetFormWorkflowStatusListResponse response = await formsServicesClient.GetFormWorkflowStatusListAsync(this.apiToken, formTemplateId, projectId);

            IEnumerable<FormWorkflowStatus> workflowStatus = this.DeserializeResponse<FormWorkflowStatus>(response.Body.GetFormWorkflowStatusListResult);

            foreach (var ws in workflowStatus)
            {
                ws.FormTemplateId = formTemplateId;
            }

            return workflowStatus;
        }

        public async Task<IEnumerable<FormsSuperStatusCountInformation>> GetFormsSuperStatusCount(int projectId, int[] formTemplateLinkIds, DateTime dateFrom, DateTime dateTo)
        {
            API_FormsServicesSoapClient formsServicesClient = await this.GetFormsServicesClient();
            GetFormsSuperStatusCountResponse response = await formsServicesClient.GetFormsSuperStatusCountAsync(this.apiToken, projectId, this.GetFormArrayOfInt(formTemplateLinkIds), dateFrom, dateTo);

            return this.DeserializeResponse<FormsSuperStatusCountInformation>(response.Body.GetFormsSuperStatusCountResult);
        }

        public async Task<IEnumerable<FormInformation>> GetGroup(string formId, string groupAlias)
        {
            API_FormsServicesSoapClient formsServicesClient = await this.GetFormsServicesClient();
            GetGroupResponse response = await formsServicesClient.GetGroupAsync(this.apiToken, formId, groupAlias);

            return this.DeserializeResponse<FormInformation>(response.Body.GetGroupResult);
        }

        public async Task<IEnumerable<FormAnswerInformation>> GetQuestionAnswer(string formId, string questionAlias)
        {
            API_FormsServicesSoapClient formsServicesClient = await this.GetFormsServicesClient();
            GetQuestionAnswerResponse response = await formsServicesClient.GetQuestionAnswerAsync(this.apiToken, formId, questionAlias);

            IEnumerable<FormAnswerInformation> result = this.DeserializeResponse<FormAnswerInformation>(response.Body.GetQuestionAnswerResult);

            foreach (FormAnswerInformation r in result)
                r.FormId = formId;

            return result;
        }

        public async Task<FormTableGroup> GetTableGroup (string formId, int formTemplateLinkId)
        {
            API_FormsServicesSoapClient formsServicesClient = await this.GetFormsServicesClient();
            GetTableGroupResponse response = await formsServicesClient.GetTableGroupAsync(this.apiToken, formId, formTemplateLinkId);

            FieldViewFormTableGroupResponse result = new FieldViewResponseFactory().Create(response.Body.GetTableGroupResult);

            foreach (FormTableGroupQuestion q in result.Questions)
            {
                q.FormId = formId;
            }

            foreach (FormTableGroupAnswer a in result.Answers)
            {
                a.FormId = formId;
            }

            return new FormTableGroup
            {
                FormId = formId,
                FormTemplateLinkId = formTemplateLinkId,
                Questions = result.Questions,
                Answers = result.Answers
            };
        }

        public async Task<FormTableGroup> GetStaticTableGroupRow (string formId, string tableGroupAlias, string rowAlias)
        {
            API_FormsServicesSoapClient formsServicesClient = await this.GetFormsServicesClient();
            GetStaticTableGroupRowResponse response = await formsServicesClient.GetStaticTableGroupRowAsync(this.apiToken, formId, tableGroupAlias, rowAlias);

            FieldViewFormTableGroupResponse result = new FieldViewResponseFactory().Create(response.Body.GetStaticTableGroupRowResult);

            return new FormTableGroup
            {
                FormId = formId,
                TableGroupAlias = tableGroupAlias,
                RowAlias = rowAlias,
                Questions = result.Questions,
                Answers = result.Answers
            };
        }

        public async Task<IEnumerable<FormTemplateInformation>> GetFormTemplateInformation(int formTemplateId)
        {
            API_FormsServicesSoapClient formsServicesClient = await this.GetFormsServicesClient();
            GetFormTemplateDetailsResponse response = await formsServicesClient.GetFormTemplateDetailsAsync(this.apiToken, formTemplateId);

            IEnumerable<FormTemplateInformation> result = this.DeserializeResponse<FormTemplateInformation>(response.Body.GetFormTemplateDetailsResult);

            return result;
        }

    }
}
