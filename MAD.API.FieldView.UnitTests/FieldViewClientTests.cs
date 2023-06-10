using MAD.API.FieldView.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAD.API.FieldView.UnitTests
{
    [TestClass]
    public class FieldViewClientTests
    {
        private FieldViewClient GetClient()
        {
            return ClientFactory.Create();
        }

        [TestMethod]
        public async Task GetProjectDetailsTest()
        {
            var client = this.GetClient();
            var projects = await client.GetProjectDetails(null, null, null);
        }

        private async Task<IEnumerable<ProjectFormTemplateInformation>> GetAllProjectTemplateAsync()
        {
            var client = this.GetClient();
            var projects = await client.GetProjectDetails(null, null, null);
            var result = new List<ProjectFormTemplateInformation>();

            foreach (var p in projects)
            {
                var templates = await client.GetProjectFormTemplates(p.Id, true);

                result.AddRange(templates);
            }

            return result;
        }

        [TestMethod]
        public async Task GetProjectFormTemplatesTest()
        {
            await this.GetAllProjectTemplateAsync();
        }

        [TestMethod]
        public async Task GetProjectFormsListTest()
        {
            await this.GetProjectFormsList();
        }

        private async Task<IEnumerable<ProjectFormsListInformation>> GetProjectFormsList()
        {
            var client = this.GetClient();
            var projects = await client.GetProjectDetails(null, null, null);
            var templates = await this.GetAllProjectTemplateAsync();

            List<ProjectFormsListInformation> result = new List<ProjectFormsListInformation>();
            DateTime startDate = DateTime.Now;

            foreach (var p in projects)
            {
                for (int i = 0; i < 8; i++)
                {
                    DateTime threeMonthDelta = startDate.AddMonths(-3 * i);

                    var projectFormsList = await client.GetProjectFormsList(p.Id, templates.Select(y => y.FormTemplateLinkId).ToArray(),
                       createdDateFrom: threeMonthDelta,
                       createdDateTo: threeMonthDelta.AddMonths(3));

                    result.AddRange(projectFormsList);
                }
            }

            return result;
        }

        [TestMethod]
        public async Task GetProjectFormsListUpdatedTest()
        {
            var client = this.GetClient();
            var projects = await client.GetProjectDetails(null, null, null);
            var templates = await this.GetAllProjectTemplateAsync();

            foreach (var p in projects)
            {
                var projectFormsList = await client.GetProjectFormsListUpdated(p.Id, templates.Select(y => y.FormTemplateLinkId).ToArray(),
                    lastmodifiedDateFrom: p.StartDate,
                    lastmodifiedDateTo: p.StartDate.Value.AddMonths(3));

                if (projectFormsList.Any())
                    break;
            }
        }

        [TestMethod]
        public async Task GetFormTest()
        {
            var client = this.GetClient();
            var projectFormsList = await this.GetProjectFormsList();

            foreach (var pfl in projectFormsList)
            {
                var form = await client.GetForm(pfl.FormId);
            }
        }

        [TestMethod]
        public async Task GetFormAnswerAuditTrailTest()
        {
            var client = this.GetClient();

            await client.GetFormAnswerAuditTrail("F23845.99", 2487648, "23845.3462");

            var projectFormsList = await this.GetProjectFormsList();

            foreach (var pfl in projectFormsList)
            {
                var form = await client.GetForm(pfl.FormId);

                foreach (var f in form)
                {
                    var auditTrail = await client.GetFormAnswerAuditTrail(f.FormId, f.FormTemplateId, f.FormAnswerId);
                }
            }
        }

        [TestMethod]
        public async Task GetFormAnswerCommentsTest()
        {
            var client = this.GetClient();
            var projectFormsList = await this.GetProjectFormsList();

            foreach (var pfl in projectFormsList)
            {
                var form = await client.GetForm(pfl.FormId);

                foreach (var f in form)
                {
                    var comments = await client.GetFormAnswerComments(f.FormAnswerId);
                }
            }
        }

        [TestMethod]
        public async Task GetFormAuditTrailTest()
        {
            var client = this.GetClient();
            var projectFormsList = await this.GetProjectFormsList();

            foreach (var pfl in projectFormsList)
            {
                var form = await client.GetForm(pfl.FormId);
                var auditTrail = await client.GetFormAuditTrail(pfl.FormId);
            }
        }

        [TestMethod]
        public async Task GetFormCommentsTest()
        {
            var client = this.GetClient();
            var projectFormsList = await this.GetProjectFormsList();

            foreach (var pfl in projectFormsList)
            {
                var form = await client.GetForm(pfl.FormId);
                var comments = await client.GetFormComments(pfl.FormId);
            }
        }

        [TestMethod]
        public async Task GetFormWorkflowStatusListTest()
        {
            var client = this.GetClient();
            var projectFormsList = await this.GetProjectFormsList();

            foreach (var pfl in projectFormsList)
            {
                var form = await client.GetForm(pfl.FormId);

                foreach (var f in form)
                {
                    var comments = await client.GetFormWorkflowStatusList(f.FormTemplateId);
                }
                
            }
        }

        [TestMethod]
        public async Task GetFormsSuperStatusCountTest()
        {
            var client = this.GetClient();
            var projects = await client.GetProjectDetails(null, null, null);
            var projectFormsList = await this.GetProjectFormsList();
            var templates = await this.GetAllProjectTemplateAsync();

            foreach (var p in projects)
            {
                foreach (var pfl in projectFormsList)
                {
                    var comments = await client.GetFormsSuperStatusCount(p.Id, 
                        templates.Select(y => y.FormTemplateLinkId).ToArray(), pfl.CreatedDate.Value, pfl.CreatedDate.Value.AddMonths(2));
                }
            }
        }

        [TestMethod]
        public async Task GetGroupTest()
        {
            var client = this.GetClient();
            var projectFormsList = await this.GetProjectFormsList();

            foreach (var pfl in projectFormsList)
            {
                var form = await client.GetForm(pfl.FormId);

                foreach (var f in form)
                {
                    var group = await client.GetGroup(f.FormId, f.Alias);
                }
                
            }
        }

        [TestMethod]
        public async Task GetQuestionAnswer()
        {
            var client = this.GetClient();
            var projectFormsList = await this.GetProjectFormsList();

            foreach (var pfl in projectFormsList)
            {
                var form = await client.GetForm(pfl.FormId);

                foreach (var f in form) 
                {
                    var answer = await client.GetQuestionAnswer(pfl.FormId, f.Question);
                }
            }
        }

        [TestMethod]
        public async Task GetTableGroup()
        {
            var client = this.GetClient();

            var projs = await this.GetProjectFormsList();

            foreach (var p in projs)
            {
                await client.GetTableGroup(p.FormId, p.FormTemplateLinkId);
            }
        }

        [TestMethod]
        public async Task GetStaticTableGroupRow()
        {
            var client = this.GetClient();
            const string formId = "F27536.4";

            var tg = await client.GetTableGroup(formId, 1934326);

            foreach (var t in tg.Questions)
            {
                await client.GetStaticTableGroupRow(formId, "Expenditure List", t.Alias);
            }

            
        }

        [TestMethod]
        public async Task GetProjectFormAttachments()
        {
            var client = this.GetClient();
            var projects = await client.GetProjectDetails(null, null, null);

            foreach (var p in projects)
            {
                var attachments = await client.GetProjectFormAttachments(p.Id, DateTime.Now.AddDays(-25), DateTime.Now);

                if (attachments.Any())
                {
                    if (1 == 1)
                    {

                    }
                }
            }
        }

        [TestMethod]
        public async Task GetProjectFormComments()
        {
            var client = this.GetClient();
            var projects = await client.GetProjectDetails(null, null, null);

            foreach (var p in projects)
            {
                var attachments = await client.GetProjectFormComments(p.Id, DateTime.Now.AddDays(-25), DateTime.Now);

                if (attachments.Any())
                {
                    if (1 == 1)
                    {

                    }
                }
            }
        }

        [TestMethod]
        public async Task GetProjectPeople()
        {
            var client = this.GetClient();
            var people = await client.GetProjectPeople(2422, 13208);
        }


        [TestMethod]
        public async Task GetPhotos()
        {
            var client = this.GetClient();
            var attachments = await client.GetProjectFormAttachments(5532, DateTimeOffset.Parse("2022-03-06T22:30:15.3047316+10:00").Date, DateTime.Now);

            foreach (var a in attachments)
            {
                var photo = await client.GetFormPhoto(a.FormId, a.Id);
            }
        }

    }
}
