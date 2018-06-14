using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Xml;
using Microsoft.Xrm.Sdk;

namespace CRMViewLoader.Models
{
    public class View
    {
        public Guid id;
        public string name;
        public string description;
        public string layoutPath;
        public string fetchPath;
        public string layoutXml;
        public string fetchXml;
        public string returnTypeCode;
        public View(string metdataPath)
        {
            string fileContents = File.ReadAllText(metdataPath);
            // Use a json deserializer to get all of the hard work out of the way
            var tmpView = JsonConvert.DeserializeObject<View>(fileContents);
            this.id = tmpView.id;
            this.name = tmpView.name;
            this.description = tmpView.description;
            string basePath = Path.GetDirectoryName(metdataPath);
            string realFetchPath = Path.Combine(basePath, tmpView.fetchPath);
            string realLayoutPath = Path.Combine(basePath, tmpView.layoutPath);
            if (!File.Exists(realFetchPath))
            {
                throw new FileNotFoundException($"Can't find fetchxml file {realFetchPath}");
            }
            if (!File.Exists(realLayoutPath))
            {
                throw new FileNotFoundException($"Can't find grid file {realLayoutPath}");
            }
            this.fetchXml = File.ReadAllText(realFetchPath);
            this.layoutXml = File.ReadAllText(realLayoutPath);
            this.returnTypeCode = GetReturnTypeCode(this.fetchXml);
        }
        public View()
        {
        }
        public string GetReturnTypeCode(string fetchXml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(fetchXml);
            var node = doc.SelectSingleNode("/fetch/entity");
            return node.Attributes["name"].Value;
        }
        public Entity AsEntity()
        {
            return new Entity("savedquery", this.id)
            {
                ["name"] = this.name,
                ["description"] = this.description,
                ["fetchxml"] = this.fetchXml,
                ["layoutxml"] = this.layoutXml,
                ["returnedtypecode"] = this.returnTypeCode,
            };
        }
    }
}
