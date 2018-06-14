using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine.Text;
using CommandLine;

namespace CRMViewLoader.Models
{
    public class CommandLineOptions
    {
        [Option('c', "connection-string", Required = true, HelpText = "Connection string to Dynamics")]
        public string ConnectionString { get; set; }

        [Option('s', "source", Required = true, HelpText = "Source to load views from")]
        public string SourceDirectory { get; set; }

        // Unused for now but will be needed in the future
        [Option("solution", Required = false, HelpText = "Solution to add new views to (currently unused)")]
        public string SolutionName { get; set; }
    }
}
