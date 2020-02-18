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

            foreach (var p in projects)
            {
                var projectFormsList = await client.GetProjectFormsList(p.Id, templates.Select(y => y.FormTemplateLinkId).ToArray(),
                    createdDateFrom: p.FinishDate.Value.AddMonths(-3),
                    createdDateTo: p.FinishDate.Value);

                result.AddRange(projectFormsList);
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

                if (form != null)
                {
                    var auditTrail = await client.GetFormAnswerAuditTrail(pfl.FormId, form.FormTemplateId, form.FormAnswerId);
                }
            }
        }
    }
}
