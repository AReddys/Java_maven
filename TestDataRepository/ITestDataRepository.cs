using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataRepository
{
    public interface ITestDataRepository
    {
        void CreateConnection();
        void CloseConnection();
        string Read(string tableName, int testCaseId, string columnName);
        List<string> ReadDataIntoList(string tableName, int testCaseId, string columnName);
        void Update();
        void Delete();
    }
}
