﻿using ConfigurationServicesEndpoint;
using FormsServicesEndpoint;
using MAD.API.FieldView.Domain;
using Newtonsoft.Json;
using ProjectServicesEndpoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace MAD.API.FieldView
{
    public class FieldViewClient
    {
        private delegate Task<string> RunApiWithPaginationCallback(int? take, int startRow);

        private const int PageSize = 500;

        private readonly string? apiToken;
        private readonly string baseUri;
        private readonly string? loginName;
        private readonly string? password;

        public FieldViewClient(string apiKey, string baseUri = null)
        {
            this.apiToken = apiKey;
            this.baseUri = baseUri;
        }

        public FieldViewClient(string loginName, string password, string baseUri = null)
        {
            this.loginName = loginName;
            this.password = password;
            this.baseUri = baseUri;
        }

        private void SetBindingTimeouts(Binding binding)
        {
            binding.CloseTimeout = TimeSpan.FromMinutes(500);
            binding.OpenTimeout = TimeSpan.FromMinutes(500);
            binding.ReceiveTimeout = TimeSpan.FromMinutes(500);
            binding.SendTimeout = TimeSpan.FromMinutes(500);
        }

        private readonly API_ProjectServicesSoapClient projectServicesSoapClient;
        private async Task<API_ProjectServicesSoapClient> GetProjectServicesClient()
        {
            API_ProjectServicesSoapClient result = this.projectServicesSoapClient;

            if (result is null)
            {
                if (!string.IsNullOrEmpty(this.baseUri))
                {
                    result = new API_ProjectServicesSoapClient(API_ProjectServicesSoapClient.EndpointConfiguration.API_ProjectServicesSoap, this.baseUri + "API_ProjectServices.asmx");
                }
                else
                {
                    result = new API_ProjectServicesSoapClient(API_ProjectServicesSoapClient.EndpointConfiguration.API_ProjectServicesSoap);
                }

            }

            this.SetBindingTimeouts(result.Endpoint.Binding);

            return result;
        }

        private readonly API_FormsServicesSoapClient formsServicesClient;
        private async Task<API_FormsServicesSoapClient> GetFormsServicesClient()
        {
            API_FormsServicesSoapClient result = this.formsServicesClient;

            if (result is null)
            {
                if (!string.IsNullOrEmpty(this.baseUri))
                {
                    result = new API_FormsServicesSoapClient(API_FormsServicesSoapClient.EndpointConfiguration.API_FormsServicesSoap, this.baseUri + "API_FormsServices.asmx");
                }
                else
                {
                    result = new API_FormsServicesSoapClient(API_FormsServicesSoapClient.EndpointConfiguration.API_FormsServicesSoap);
                }
            }

            this.SetBindingTimeouts(result.Endpoint.Binding);

            return result;
        }

        private readonly API_ConfigurationServicesSoapClient configurationServicesClient;
        private async Task<API_ConfigurationServicesSoapClient> GetConfigurationServicesClient()
        {
            API_ConfigurationServicesSoapClient result = this.configurationServicesClient;

            if (result is null)
            {
                if (!string.IsNullOrEmpty(this.baseUri))
                {
                    result = new API_ConfigurationServicesSoapClient(API_ConfigurationServicesSoapClient.EndpointConfiguration.API_ConfigurationServicesSoap, this.baseUri + "API_ConfigurationServices.asmx");
                }
                else
                {
                    result = new API_ConfigurationServicesSoapClient(API_ConfigurationServicesSoapClient.EndpointConfiguration.API_ConfigurationServicesSoap);
                }
            }


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

        private IEnumerable<TEntity> DeserializeResponse<TEntity>(string json)
        {
            FieldViewResponse<TEntity> response = new FieldViewResponseFactory().Create<TEntity>(json);

            return response.Entities;
        }

        public async Task<IEnumerable<ProjectInformation>> GetProjectDetails(string projectName = null,
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

            foreach (ProjectFormTemplateInformation r in result)
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

        public async Task<IEnumerable<CommentInformation>> GetProjectFormComments(int projectId, DateTime lastModifiedDateFrom, DateTime lastModifiedDateTo)
        {
            API_FormsServicesSoapClient formsServicesClient = await this.GetFormsServicesClient();
            var response = await formsServicesClient.GetProjectFormsCommentsAsync(this.apiToken, projectId, new FormsServicesEndpoint.ArrayOfInt(), lastModifiedDateFrom, lastModifiedDateTo, null, null);

            IEnumerable<CommentInformation> comments = this.DeserializeResponse<CommentInformation>(response.Body.GetProjectFormsCommentsResult);

            return comments;
        }

        public async Task<IEnumerable<FormWorkflowStatus>> GetFormWorkflowStatusList(int formTemplateId, int? projectId = null)
        {
            API_FormsServicesSoapClient formsServicesClient = await this.GetFormsServicesClient();
            GetFormWorkflowStatusListResponse response = await formsServicesClient.GetFormWorkflowStatusListAsync(this.apiToken, formTemplateId, projectId);

            IEnumerable<FormWorkflowStatus> workflowStatus = this.DeserializeResponse<FormWorkflowStatus>(response.Body.GetFormWorkflowStatusListResult);

            foreach (FormWorkflowStatus ws in workflowStatus)
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

        public async Task<FormTableGroup> GetTableGroup(string formId, int formTemplateLinkId)
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

        public async Task<FormTableGroup> GetTableGroupByAlias(string formId, string tableGroupAlias)
        {
            API_FormsServicesSoapClient formsServicesClient = await this.GetFormsServicesClient();
            GetTableGroupByAliasResponse response = await formsServicesClient.GetTableGroupByAliasAsync(this.apiToken, formId, tableGroupAlias);

            FieldViewFormTableGroupResponse result = new FieldViewResponseFactory().Create(response.Body.GetTableGroupByAliasResult);

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
                TableGroupAlias = tableGroupAlias,
                Questions = result.Questions,
                Answers = result.Answers
            };
        }

        public async Task<FormTableGroup> GetStaticTableGroupRow(string formId, string tableGroupAlias, string rowAlias)
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

        public async Task<IEnumerable<FormAttachment>> GetFormAttachments(string formId, string answerId = null)
        {
            API_FormsServicesSoapClient formsServicesClient = await this.GetFormsServicesClient();
            GetFormAttachmentsResponse response = await formsServicesClient.GetFormAttachmentsAsync(this.apiToken, formId, answerId);

            IEnumerable<FormAttachment> result = this.DeserializeResponse<FormAttachment>(response.Body.GetFormAttachmentsResult);

            foreach (FormAttachment r in result)
            {
                r.FormId = formId;
            }

            return result;
        }

        public async Task<IEnumerable<FormsListAttachmentInformation>> GetProjectFormAttachments(int projectId, DateTime lastmodifiedDateFrom, DateTime lastmodifiedDateTo)
        {
            API_FormsServicesSoapClient formsServicesClient = await this.GetFormsServicesClient();
            GetProjectFormsAttachmentsResponse response = await formsServicesClient.GetProjectFormsAttachmentsAsync(this.apiToken, projectId, new FormsServicesEndpoint.ArrayOfInt(), lastmodifiedDateFrom, lastmodifiedDateTo, null, null);

            IEnumerable<FormsListAttachmentInformation> result = this.DeserializeResponse<FormsListAttachmentInformation>(response.Body.GetProjectFormsAttachmentsResult);

            return result;
        }

        public async Task<IEnumerable<FormDocument>> GetFormDocument(string formId, int documentId)
        {
            API_FormsServicesSoapClient formsServicesClient = await this.GetFormsServicesClient();
            GetFormDocumentResponse response = await formsServicesClient.GetFormDocumentAsync(this.apiToken, formId, documentId);

            IEnumerable<FormDocument> result = this.DeserializeResponse<FormDocument>(response.Body.GetFormDocumentResult);

            foreach (FormDocument r in result)
            {
                r.FormId = formId;
            }

            return result;
        }

        public async Task<IEnumerable<FormPhoto>> GetFormPhoto(string formId, string mediaId)
        {
            API_FormsServicesSoapClient formsServicesClient = await this.GetFormsServicesClient();
            GetFormPhotoResponse response = await formsServicesClient.GetFormPhotoAsync(this.apiToken, formId, mediaId);

            IEnumerable<FormPhoto> result = this.DeserializeResponse<FormPhoto>(response.Body.GetFormPhotoResult);

            foreach (FormPhoto r in result)
            {
                r.FormId = formId;
            }

            return result;
        }

        public async Task<FormPdf> GetFormPdf(string formId, bool showActions, bool showAnsweredBy, bool showAttachedComments, bool showAttachedDocuments, bool showAttachedImages, bool showStatusAuditTrail, string imageSize)
        {
            API_FormsServicesSoapClient formsServicesClient = await this.GetFormsServicesClient();

            PDFImageSize size = (PDFImageSize)Enum.Parse(typeof(PDFImageSize), imageSize);
            GetFormPdfResponse response = await formsServicesClient.GetFormPdfAsync(this.apiToken, formId, showActions, showAnsweredBy, showAttachedComments, showAttachedDocuments, showAttachedImages, showStatusAuditTrail, size);

            FormPdf result = JsonConvert.DeserializeObject<FormPdf>(response.Body.GetFormPdfResult);
            result.FormId = formId;

            return result;
        }
    }
}
