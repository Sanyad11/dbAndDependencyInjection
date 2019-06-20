using System;

namespace dbAndDependencyInjection
{
    interface IsqlConnector : IDisposable
    {
        bool AddDataToTableSpeciality(int id, string name, out string exept);
        bool AddDataToTableSpeciality(string name, out string exept);
        void SQLConnect(string baseName, out string exept);
        bool AddDataToTableWorker(int id, string name, int? bossId, int SpecialityId, int IsBoss, out string exept);
        bool ExecuteQuery(string query, string message, out string exept);
        bool CreateTableSpeciality(out string exept);
        bool CreateTableWorker(out string exept);
        string ShowTableBosses(out string exept);
        string ShowTableWorkers(out string exept);
    }
}