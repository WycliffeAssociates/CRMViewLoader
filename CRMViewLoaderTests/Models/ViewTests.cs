using CRMViewLoader.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMViewLoaderTests.Models
{
    [TestClass]
    public class ViewTests
    {
        [TestMethod]
        public void AsEntityTests()
        {
            View target = new View()
            {
                name = "Test View",
                description = "Description",
                fetchXml = "<fetch></fetch>",
                layoutPath = "<grid></grid>",
                returnTypeCode = "entity",
                id = Guid.NewGuid()
            };
            Entity created = target.AsEntity();
            Assert.AreEqual(target.name, created["name"]);
            Assert.AreEqual(target.description, created["description"]);
            Assert.AreEqual(target.fetchXml, created["fetchxml"]);
            Assert.AreEqual(target.layoutXml, created["layoutxml"]);
        }

        [TestMethod]
        public void TestGetEntityTypeFromFetch()
        {
            View target = new View();
            string entityName = "entity";
            Assert.AreEqual(entityName, target.GetReturnTypeCode($"<fetch><entity name=\"{entityName}\"></entity></fetch>"));
        }
    }
}
