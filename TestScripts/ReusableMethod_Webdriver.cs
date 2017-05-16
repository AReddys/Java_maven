using Entities;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TestScripts
{
    public class ReusableMethod_Webdriver : TestCaseProperties, IReusableMethods
    {
        public static IWebDriver driver;
        public static By by;
        /// <summary>
        /// Below method is used to iterate through the XML file, to fetch element information/details. 
        /// </summary>
        /// <param name="ORName">XML Name from which element need to be searched</param>
        /// <param name="elementName">Element Name for which details need to be fetched from XML</param>
        public void GetElementDetails(string ORName, string elementName)
        {
            string xmlFile = TestCaseProperties.workSpaceRelativePath() + "\\ObjectRepository\\LPI\\" + ORName + ".xml";
            XmlTextReader textReader = new XmlTextReader(xmlFile);
            XmlDocument xmlORdocument = new XmlDocument();
            bool elementIdentified = false;
            xmlORdocument.Load(xmlFile);
            bool browserDetailsExists = false;
            XmlNodeList elemList = xmlORdocument.GetElementsByTagName("Element");
            foreach (XmlNode node in elemList)
            {
                XmlElement parentElement = (XmlElement)node;
                bool identifierInformation = false;
                if (parentElement.Attributes["Name"].Value.Trim() == elementName)
                {
                    //Console.WriteLine(parentElement.Attributes["Name"].Value); // It prints element name
                    elementIdentified = true;
                    by = null;

                    identifierInformation = true;
                    XmlNodeList locator_ElementList = parentElement.GetElementsByTagName("Locator");
                    foreach (XmlNode locatorText in locator_ElementList)
                    {
                        // System.Diagnostics.Debug.WriteLine("----------" + locatorText.InnerText); // Class name
                        XmlAttributeCollection locatorAttributes = locatorText.Attributes;
                        //Below condition will work when for an element we have  browser type as an attribute.
                        if (locatorAttributes["BrowserType"] != null)
                        {
                            foreach (XmlAttribute attributeDetailsForParentNode in locatorAttributes)
                            {
                                if (attributeDetailsForParentNode.Value.ToLower().Equals(browserType))
                                {
                                    browserDetailsExists = true;
                                    SelectAndAssignLocatorTypeAndValue(locatorAttributes);
                                }
                            }
                        }

                        else
                        {
                            SelectAndAssignLocatorTypeAndValue(locatorAttributes);
                        }
                    }
                }
                if (identifierInformation) { break; }
            }
            //Below consition is executed when there element is not found in the XML document.
            if (!elementIdentified)
            {
                System.Diagnostics.Debug.WriteLine("Element is not present in " + ORName + " XML File.");
            }
            if (!browserDetailsExists)
            {

                System.Diagnostics.Debug.WriteLine("Browser attribute details is not present in " + ORName + " XML File.");
            }
        }


        public void SelectAndAssignLocatorTypeAndValue(XmlAttributeCollection locatorAttributes)
        {
            foreach (XmlAttribute locatorAttributeDetails in locatorAttributes)
            {
                #region
                switch (locatorAttributeDetails.Name.ToLower())
                {
                    case "id":
                        by = By.Id(locatorAttributeDetails.Value);
                        break;

                    case "name":
                        by = By.Name(locatorAttributeDetails.Value);
                        break;

                    case "linktext":
                        by = By.LinkText(locatorAttributeDetails.Value);
                        break;

                    case "partiallinktext":
                        by = By.PartialLinkText(locatorAttributeDetails.Value);
                        break;

                    case "xpath":
                        by = By.XPath(locatorAttributeDetails.Value);
                        break;

                    case "classname":
                        by = By.ClassName(locatorAttributeDetails.Value);
                        break;

                    case "cssselector":
                        by = By.CssSelector(locatorAttributeDetails.Value);
                        break;

                    case "tagname":
                        by = By.TagName(locatorAttributeDetails.Value);
                        break;

                }
                #endregion
            }
        }

       
        public ReusableMethod_Webdriver()
        {
            string applicationName = browserType;
            if (applicationName.Equals("firefox"))
            {
                // FirefoxDriverService service = FirefoxDriverService.CreateDefaultService();
                // service.FirefoxBinaryPath = @"C:\Users\Administrator\documents\visual studio 2013\Projects\encoreSSP\TestScripts\Browsers\geckodriver.exe";
                //// FirefoxDriver driver = new FirefoxDriver(service);
                // //System.Environment.SetEnvironmentVariable("webdriver.gecko.driver",@"C:\Users\Administrator\documents\visual studio 2013\Projects\encoreSSP\TestScripts\Browsers\geckodriver.exe");
                //  driver = new FirefoxDriver(service);
                FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(@"C:\Users\Administrator\documents\visual studio 2013\Projects\encoreSSP\TestScripts\Browsers", "geckodriver.exe");
                //  service.Port = 64444;
                service.FirefoxBinaryPath = @"C:\Users\Administrator\documents\visual studio 2013\Projects\encoreSSP\TestScripts\Browsers\geckodriver.exe";
                driver = new FirefoxDriver(service);
            }
            else if (applicationName.Equals("ie"))
            {
                driver = new InternetExplorerDriver(@"C:\Users\Administrator\Downloads\IEDriverServer_x64_3.0.0");
            }
        }
        public void EnterData(string ORName, string ElementName, string Data)
        {
            GetElementDetails(ORName, ElementName);
            driver.FindElement(by).SendKeys(Data);
        }

        public void Click(string ORName, string ElementName)
        {
            GetElementDetails(ORName, ElementName);
            driver.FindElement(by).Click();
        }

        public void Launch()
        {
            driver.Navigate().GoToUrl(applicationName);
            driver.Manage().Window.Maximize();
        }


        public void CloseApplication()
        {
            driver.Quit();
        }
    }
}
