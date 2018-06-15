using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CRMViewLoader.Models;
using CommandLine;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;

namespace CRMViewLoader
{
    public class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(opts => Run(opts));
        }
        static void Run(CommandLineOptions args)
        {
            try
            {
                CrmServiceClient service = new CrmServiceClient(args.ConnectionString);
                if (!string.IsNullOrEmpty(service.LastCrmError))
                {
                    throw new Exception("Error connecting to CRM");
                }
                var views = FindAllViewsInFolder(args.SourceDirectory);
                UpdateViewsInCRM(views, service);
                Console.WriteLine("All done");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in processing");
                Console.WriteLine(ex.Message);
                Console.WriteLine("If you believe this is a problem with the application please report it here: https://github.com/wycliffeassocaites/CRMViewLoader");
                Environment.Exit(1);
            }
        }

        public static void UpdateViewsInCRM(List<View> views, IOrganizationService service)
        {
            foreach (var i in views)
            {
                Console.WriteLine($"Processing {i.name}");
                service.Update(i.AsEntity());
            }

            Console.WriteLine("Publishing");
            PublishAllXmlRequest publishRequest = new PublishAllXmlRequest();
            service.Execute(publishRequest);
        }

        private static List<View> FindAllViewsInFolder(string path)
        {
            List<View> output = new List<View>();
            foreach(var i in Directory.EnumerateFiles(path, "*.json", SearchOption.AllDirectories))
            {
                output.Add(new View(i));
            }
            return output;
        }
    }
}
