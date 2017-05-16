using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Entities;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Reflection;

namespace TestDataRepository.Excel
{
   public class ExcelOprations : TestCaseProperties, ITestDataRepository
    {
        OleDbConnection excelConnection;
        static DataSet dataSet = new DataSet();


        public void Update()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void CreateConnection()
        {
            string fileName =TestCaseProperties.workSpaceRelativePath() + @"TestDataRepository\Excel\Files\AutomationTestData.xls";
            string connectionString = string.Format("provider=Microsoft.Jet.OLEDB.4.0; data source={0};Extended Properties=Excel 8.0;", fileName);
            excelConnection = new OleDbConnection(connectionString);
            excelConnection.Open();
        }

        public void CloseConnection()
        {
            excelConnection.Close();
            excelConnection = null;
        }

        public string Read(string tableName, int testCaseId, string columnName)
        {
            return dataSet.Tables[tableName + "$"].AsEnumerable().Where(row => Convert.ToInt32(row["TestCaseID"]) == testCaseId && row["BrowserType"].Equals(browserType)).Select(row => row.Field<string>(columnName)).First();

        }
        /// <summary>
        /// Below method is used to fetch when we have multiple data from excel. Ex: Selecting multiple contries in a selected dropdownlist
        /// </summary>
        /// <param name="tableName"> Sheet Name in the Excel workbook</param>
        /// <param name="columnName">Column Name in the Excel Sheet for which data need to be fetched</param>
        /// <returns></returns>
        public List<string> ReadDataIntoList(string tableName, int testCaseId, string columnName)
        {
            return dataSet.Tables[tableName + "$"].AsEnumerable().Where(row => Convert.ToInt32(row["TestCaseID"]) == testCaseId && row["BrowserType"].Equals(browserType)).Select(row => row.Field<string>(columnName)).ToList();
        }

        /// <summary>
        /// Below method is to assign excel sheets as DataTable to DataSet.
        /// </summary>
        public void fetchExcelDataIntoDataSet()
        {
            CreateConnection();
            foreach (var sheetName in GetExcelSheetNames())
            {
                DataTable dataTable = new DataTable();
                string query = string.Format("SELECT * FROM [{0}]", sheetName);
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, excelConnection);
                adapter.Fill(dataTable);
                dataTable.TableName = sheetName;
                dataSet.Tables.Add(dataTable);
            }
            CloseConnection();
        }

        /// <summary>
        /// 
        /// Below method is to fetch all the sheet names of Excel workbook
        /// </summary>
        /// <returns></returns>
        public string[] GetExcelSheetNames()
        {
            DataTable dt = null;
            dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (dt == null)
            {
                return null;
            }

            String[] excelSheetNames = new String[dt.Rows.Count];
            int tableCount = 0;

            foreach (DataRow row in dt.Rows)
            {
                excelSheetNames[tableCount] = row["TABLE_NAME"].ToString();
                tableCount++;
            }

            return excelSheetNames;
        }
    }
}
