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
            string token = File.ReadAllText("APIToken.txt");

            FieldViewClient client = new FieldViewClient(token);

            return client;
        }

        [TestMethod]
        public async Task GetProjectDetailsTest()
        {
            var client = this.GetClient();
            var projects = await client.GetProjectDetails(null, new int[] { 31854, 31852, 31855, 31856, 31827, 31814 }, null);
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

    }
}
