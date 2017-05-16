using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using HelperMethods;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using Entities;
using System.Configuration;
using System.Reflection;
using System.Diagnostics;
using System.Threading;
using TestDataRepository.Excel;


namespace TestScripts
{
    /// <summary>
    /// Summary description for LPIDeficiencyFile
    /// </summary>
    [TestClass]
    public class LPIDeficiencyFile
    {
        public LPIDeficiencyFile()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        ExcelOprations excelOprations = new ExcelOprations();
        private TestContext testContextInstance;
        int testCaseID;
        IReusableMethods objWebDriver = new ReusableMethod_Webdriver();
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
           
        //}
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize() {
         excelOprations.fetchExcelDataIntoDataSet();
        }
        
        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup() { objWebDriver.CloseApplication(); }

        #endregion
        
        private void GetTestCaseID()
        {
            testCaseID = 0;
            try
            {
                var stackTrace = new StackTrace(Thread.CurrentThread, false);
                StackFrame[] stack = stackTrace.GetFrames();
                MethodBase method = null;
                String parentPath = TestContext.FullyQualifiedTestClassName;
                foreach (StackFrame stackFrame in stack)
                {
                    //need to match the testcontext's parent path with a method bases' parent path
                    //this will not match the test method directly, but will allow the type.gettype to work
                    //from there, I can match the method name and custom attributes to find the test case ID.
                    MethodBase methodBase = stackFrame.GetMethod();
                    Type classType = methodBase.ReflectedType;
                    if (classType.FullName.ToLower() == parentPath.ToLower())
                    {
                        method = classType.GetMethod(TestContext.TestName);
                        IEnumerable<Attribute> attributes = method.GetCustomAttributes(typeof(Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute));
                        foreach (Attribute attrib in attributes)
                        {
                            int categoryCount = ((Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute)attrib).TestCategories.Count;
                            for (int i = 0; i < categoryCount; i++)
                            {
                                String categoryValue = ((Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute)attrib).TestCategories[i];
                                if (categoryValue.StartsWith("TCID:"))
                                {
                                    testCaseID = Convert.ToInt32(categoryValue.Substring("TCID:".Length).Trim());
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
            }
            catch
            {
                //can land here for a lot of really good and valid reasons
            }
        }
        [TestMethod]
        [TestCategory("Regression: X-MEN"), TestCategory("TCID: 100"), TestCategory("Story: 69158")]
        public void TestMethod1()
        {
            
            GetTestCaseID();
            objWebDriver.Launch();
            objWebDriver.EnterData("MainPage_Webdriver", "Textbox_UserName", excelOprations.Read("Mercury", testCaseID, "UserName"));
            objWebDriver.EnterData("MainPage_Webdriver", "Textbox_Password", excelOprations.Read("Mercury", testCaseID, "Password"));
            objWebDriver.Click("MainPage_Webdriver", "Button_SignIn");
        }

    }
}

