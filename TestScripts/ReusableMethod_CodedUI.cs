using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UITesting.HtmlControls;
using System.Reflection;
using Entities;
using System.Xml;
using OpenQA.Selenium;
using System.IO;
namespace TestScripts
{
    class ReusableMethod_CodedUI : TestCaseProperties, IReusableMethods
    {
        public static BrowserWindow window; //Variable is used by the Coded UI.
        public static object codedUIControlReturndata; // Variable is used to get return value of an element in Coded UI.
        public static object codedUIControlProperties = null; // Variable is used by codeUI code to hold properties information.
        public static By by;

        public ReusableMethod_CodedUI()
        {

        }

        /// <summary>
        /// Below method is used to iterate through the XML file, to fetch element information/details. 
        /// </summary>
        /// <param name="ORName">XML Name from which element need to be searched</param>
        /// <param name="elementName">Element Name for which details need to be fetched from XML</param>
        public void GetElementDetails(string ORName, string elementName)
        {
            string xmlFile = TestCaseProperties.workSpaceRelativePath() + "\\" + ORName + ".xml";
            XmlDocument xmlORdocument = new XmlDocument();
            xmlORdocument.Load(xmlFile);
            XmlNodeList elemList = xmlORdocument.GetElementsByTagName("Element");
            foreach (XmlNode node in elemList)
            {
                XmlElement parentElement = (XmlElement)node;
                bool identifierInformation = false;
                if (parentElement.Attributes["Name"].Value.Trim() == elementName)
                {
                    //Console.WriteLine(parentElement.Attributes["Name"].Value); // It prints element name
                    codedUIControlProperties = null;
                    by = null;
                    #region
                    XmlNodeList locator_TypeList = parentElement.GetElementsByTagName("BrowserType");
                    foreach (XmlNode TypeNode in locator_TypeList)
                    {
                        XmlElement element_BrowserType = (XmlElement)TypeNode;
                        if (element_BrowserType.Attributes["Name"].Value == browserType)
                        {
                            identifierInformation = true;
                            XmlNodeList locator_ElementList = element_BrowserType.GetElementsByTagName("Locator");
                            foreach (XmlNode locatorText in locator_ElementList)
                            {
                                // System.Diagnostics.Debug.WriteLine("----------" + locatorText.InnerText); // Class name

                                XmlAttributeCollection locatorAttributes = locatorText.Attributes;
                                Hashtable identifiers = new Hashtable();
                                foreach (XmlAttribute locatorAttributeDetails in locatorAttributes)
                                {
                                    //   System.Diagnostics.Debug.Write(locatorAttributeDetails.Name + " :");
                                    //  System.Diagnostics.Debug.WriteLine(locatorAttributeDetails.Value + "  ");
                                    identifiers.Add(locatorAttributeDetails.Name, locatorAttributeDetails.Value);
                                }
                                switch (locatorText.InnerText.ToLower())
                                {
                                    #region
                                    case "htmledit":
                                        CodedUIClassMethod<HtmlEdit>(identifiers);
                                        break;

                                    case "htmlareahyperlink":
                                        CodedUIClassMethod<HtmlHyperlink>(identifiers);
                                        break;

                                    case "htmlbutton":
                                        CodedUIClassMethod<HtmlButton>(identifiers);
                                        break;

                                    case "htmlcheckbox":
                                        CodedUIClassMethod<HtmlCheckBox>(identifiers);
                                        break;

                                    case "htmlradiobutton":
                                        CodedUIClassMethod<HtmlRadioButton>(identifiers);
                                        break;

                                    case "htmltable":
                                        CodedUIClassMethod<HtmlTable>(identifiers);
                                        break;

                                    case "htmldiv":
                                        CodedUIClassMethod<HtmlDiv>(identifiers);
                                        break;
                                    case "htmlinputbutton":
                                        CodedUIClassMethod<HtmlInputButton>(identifiers);
                                        break;

                                    default:
                                        break;
                                    #endregion
                                }
                                identifiers.Clear();
                            }
                        }//End of If block
                    }
                    #endregion
                }
                if (identifierInformation) { break; }
            }
        }



        ///// <summary>
        ///// Below method is to get Assembly OUT folder path.
        ///// </summary>
        ///// <returns></returns>
        //public string GetAssemblyLocationPath()
        //{
        //    string fileLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
        //    return fileLocation.Substring(0, fileLocation.Substring(0, fileLocation.Length - 1).LastIndexOf('\\'));

        //}
        /// <summary>
        /// Below method is used by Coded UI tool, to assign property information/ control properties details.
        /// </summary>
        /// <typeparam name="T">HTML Control Name</typeparam>
        /// <param name="PropertyDetails"></param>
        public static void CodedUIClassMethod<T>(Hashtable PropertyDetails) where T : HtmlControl
        {
            HtmlControl control;

            //Below if-else condition is used when we have Parent class and Child Class for an element/control.
            if (codedUIControlProperties == null)
            {
                control = (T)Activator.CreateInstance(typeof(T), new object[] { window });
            }
            else
            {
                control = (T)Activator.CreateInstance(typeof(T), new object[] { codedUIControlProperties });
            }

            Type controlType = typeof(T);
            PropertyInfo[] controlProperties = controlType.GetProperties();

            foreach (DictionaryEntry propertyName in PropertyDetails)
            {
                foreach (PropertyInfo propertydetails in controlProperties)
                {
                    if (propertydetails.Name.Equals(propertyName.Key.ToString()))
                    {
                        if (propertyName.Value.ToString().ToLower().Equals("returnvalue"))
                        {
                            codedUIControlReturndata = control.GetProperty(propertydetails.Name);
                        }
                        else
                        {
                            control.SearchProperties[propertydetails.Name] = propertyName.Value.ToString();
                        }

                        break;
                    }
                }
            }
            codedUIControlProperties = control;
        }


        public void EnterData(string ORName, string ElementName, string Data)
        {
            GetElementDetails(ORName, ElementName);
            Keyboard.SendKeys((UITestControl)codedUIControlProperties, Data);
        }

        public void Click(string ORName, string ElementName)
        {
            GetElementDetails(ORName, ElementName);
            Mouse.Click((UITestControl)codedUIControlProperties);
        }

        public void Launch()
        {
            window = BrowserWindow.Launch(applicationName);
        }


        public void CloseApplication()
        {
            throw new NotImplementedException();
        }
    }
}
