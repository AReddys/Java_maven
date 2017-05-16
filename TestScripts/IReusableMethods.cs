using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestScripts
{
    interface IReusableMethods
    {
        void EnterData(string ORName, string ElementName, string Data);
        void Click(string ORName, string ElementName);
        void Launch();
        void GetElementDetails(string ORName, string elementName);
        void CloseApplication();
    }
}
