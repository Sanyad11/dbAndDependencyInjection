using System;
using System.Data.SQLite;
using System.IO;
using Dapper;



namespace dbAndDependencyInjection
{
    public class DapperSQLConnector : IsqlConnector
    {
        private static SQLiteConnection _dbConnection;

        public bool AddDataToTableSpeciality(int id, string name, out string exept)
        {
            return this.ExecuteQuery(@"INSERT INTO Speciality(id, Name) VALUES (" + id + ",'" + name + "'); ",
                       "The data was added",
                       out exept
                       );
        }

        public bool AddDataToTableSpeciality(string name, out string exept)
        {
            return this.ExecuteQuery(@"INSERT INTO Speciality( Name) VALUES (" + name + "'); ",
                        "The data was added",
                         out exept
                         );
        }

        public bool AddDataToTableWorker(int id, string name, int? bossId, int SpecialityId, int IsBoss, out string exept)
        {
             return this.ExecuteQuery(@"INSERT INTO Worker(id, Name,BossId,SpecialityId,IsBoss) 
                        VALUES (" + id + ",'" + name + "'," + (bossId == null ? "'NULL'" : bossId.ToString()) + "," + SpecialityId + "," + IsBoss + "); ",
                        "The data was added",
                        out exept
                        );
        }

        public void SQLConnect(string baseName, out string exept)
        {
            var dbFilePath = "../../../DB.db";
            if (!File.Exists(dbFilePath))
            {
                SQLiteConnection.CreateFile(dbFilePath);
            }
            _dbConnection = new SQLiteConnection(string.Format(
                "Data Source={0};Version=3;", dbFilePath));
            _dbConnection.Open();
            exept = String.Empty;
        }
        
        public bool CreateTableSpeciality(out string exept)
        {
            return this.ExecuteQuery(@"CREATE TABLE [Speciality] (
                    [id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
                    [Name] char(100) NOT NULL
                    );",
                    "Table Speciality was created",
                    out exept
                    );
        }

        public bool CreateTableWorker(out string exept)
        {
            return this.ExecuteQuery(@"CREATE TABLE [Worker] (
                    [id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
                    [Name] char(100) NOT NULL,
                    [BossId] int,
                    [SpecialityId] int NOT NULL,
                    [IsBoss] int NOT NULL
                    );",
                    "Table Worker was created",
                    out exept
                    );
        }

        public void Dispose()
        {
            _dbConnection.Close();
        }

        public string ShowTableBosses(out string exept)
        {
            try
            {
                var worker = _dbConnection.Query<WorkerIsBoss>("SELECT W.Name AS Name, S.Name AS Speciality " +
                                            "FROM Worker AS W " +
                                            "JOIN Speciality AS S " +
                                            "ON S.Id = W.SpecialityId " +
                                            "WHERE W.IsBoss = 1");
                string res = "Workers:\n";
                foreach (var w in worker)
                {
                    res += w.Name + " is " + w.Speciality + " and boss\n";
                }
                exept = "\n Bosses showed\n";
                return res;
            }
            catch (Exception ex)
            {
                exept = "\n" + ex.Message + "\n";
                return "";
            }
             
        }

        public string ShowTableWorkers(out string exept)
        {
            try
            {
                var worker = _dbConnection.Query<WorkerNotBoss>("SELECT W.Name AS Name,B.Name AS Boss, S.Name AS Speciality " +
                                                "FROM Worker AS W " +
                                                "JOIN Worker AS B " +
                                                "ON B.Id = W.BossId " +
                                                "JOIN Speciality AS S " +
                                                "ON S.Id = W.SpecialityId");
                string res = "Workers:\n";
                foreach (var w in worker)
                {
                    res += w.Name + " is " + w.Speciality + " has boss " + w.Boss + "\n";
                }
                exept = "\n Workers showed\n";
                return res;
            }
            catch (Exception ex)
            {
                exept = "\n" + ex.Message + "\n";
                return "";
            }
        }

        public bool ExecuteQuery(string query, string message, out string exept)
        {

            try
            {
                _dbConnection.Execute(query);

                exept = $"\n{message}\n";
                return true;
            }
            catch (Exception ex)
            {
                exept = "\n" + ex.Message + "\n";
                return false;
            }
        }
    }
}
