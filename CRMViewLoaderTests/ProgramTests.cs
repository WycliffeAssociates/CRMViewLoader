using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CRMViewLoader.Models;
using Microsoft.Xrm.Sdk;
using FakeXrmEasy;
using CRMViewLoader;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk.Messages;
using CRMViewLoaderTests.TestHelpers;

namespace CRMViewLoaderTests
{
    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        public void UpdateViewsInCrmTest()
        {
            View view = new View()
            {
                name = "Name",
                description = "Description",
                fetchXml = "fetch",
                layoutXml = "layout",
                id = Guid.NewGuid(),
            };
            Entity existingView = new Entity("savedquery", view.id)
            {
                ["name"] = "Name before",
                ["description"] = "Description before",
                ["fetchxml"] = "Before fetch",
                ["layoutxml"] = "Before layout"
            };
            XrmFakedContext context = new XrmFakedContext();
            PublishAllXmlRequestHandler publishHandler = new PublishAllXmlRequestHandler();
            context.AddFakeMessageExecutor<PublishAllXmlRequest>(publishHandler);
            context.Initialize(existingView);
            IOrganizationService service = context.GetOrganizationService();
            Program.UpdateViewsInCRM(new List<View>() { view }, service);
            QueryExpression query = new QueryExpression("savedquery");
            query.ColumnSet = new ColumnSet(true);
            var result = service.RetrieveMultiple(query);
            Assert.AreEqual(1, result.Entities.Count);
            Entity updatedView = result.Entities[0];
            Assert.AreEqual(view.name, updatedView["name"]);
            Assert.AreEqual(view.description, updatedView["description"]);
            Assert.AreEqual(view.fetchXml, updatedView["fetchxml"]);
            Assert.AreEqual(view.layoutXml, updatedView["layoutxml"]);
            Assert.AreEqual(true, publishHandler.published);
        }
    }
}
