using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Entities
{
    public class TestCaseProperties
    {
        public string applicationName = ConfigurationManager.AppSettings["ApplicationName"].ToLower();
        public string browserType = ConfigurationManager.AppSettings["BrowserType"].ToLower();
        public string applicationExecution { get; set; }
        public string fileLocation;
        //public static string workSpaceRelativePath = GetAssemblyLocationPath();

        public static string workSpaceRelativePath()
        {


            string workSpaceLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            if (workSpaceLocation.Contains("\\bin"))
            { workSpaceLocation = (System.Reflection.Assembly.GetExecutingAssembly().Location).Split(new string[] { "\\bin" }, StringSplitOptions.None)[0];
            workSpaceLocation = workSpaceLocation.Remove(workSpaceLocation.LastIndexOf('\\') + 1);
            }
            else if (workSpaceLocation.Contains("\\TestResults"))
            { workSpaceLocation = (System.Reflection.Assembly.GetExecutingAssembly().Location).Split(new string[] { "\\TestResults" }, StringSplitOptions.None)[0]; }
            
            return workSpaceLocation;

        }

    }
}
