using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;


namespace dbAndDependencyInjection
{
    public class ADOsqlConnector : IsqlConnector
    {


        private SQLiteConnection connection;
        private SQLiteCommand command;


        public void SQLConnect(string baseName, out string exept)
        {
            //string baseName = "CompanyWorkers.db3";
            try
            {
                SQLiteConnection.CreateFile(baseName);
                SQLiteFactory factory = (SQLiteFactory)DbProviderFactories.GetFactory("System.Data.SQLite");
                exept = "\nCreate db file corect\n";
                try
                {
                    connection = (SQLiteConnection)factory.CreateConnection();

                    connection.ConnectionString = "Data Source = " + baseName;
                    connection.Open();

                    command = new SQLiteCommand(connection);

                    exept = "\nConect to db corect\n";


                }
                catch (Exception ex2)
                {
                    exept = "\n" + ex2.Message + "\n";
                }
            }
            catch (Exception ex1)
            {
                exept = "\n" + ex1.Message + "\n";
            }

        }

        public bool CreateTableWorker(out string exept)
        {
            try
            {
                command.CommandText = @"CREATE TABLE [Worker] (
                    [id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
                    [Name] char(100) NOT NULL,
                    [BossId] int,
                    [SpecialityId] int NOT NULL,
                    [IsBoss] int NOT NULL
                    );";
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
                exept = "\nTable Worker was created\n";
                return true;
            }
            catch (Exception ex)
            {
                exept = "\n" + ex.Message + "\n";
                return false;
            }
        }

        public bool CreateTableSpeciality(out string exept)
        {
            try
            {
                command.CommandText = @"CREATE TABLE [Speciality] (
                    [id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
                    [Name] char(100) NOT NULL
                    );";
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
                exept = "\nTable Speciality was created\n";
                return true;
            }
            catch (Exception ex)
            {
                exept = "\n" + ex.Message + "\n";
                return false;
            }
        }

        public bool AddDataToTableSpeciality(int id, string name, out string exept)
        {
            try
            {
                command.CommandText = @"INSERT INTO Speciality(id, Name) VALUES (" + id + ",'" + name + "'); ";
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();

                exept = "\n Data added to Speciality table\n";
                return true;
            }
            catch (Exception ex)
            {
                exept = "\n" + ex.Message + "\n";
                return false;
            }
        }

        public bool AddDataToTableSpeciality(string name, out string exept)
        {
            try
            {
                command.CommandText = @"INSERT INTO Speciality( Name) VALUES (" + name + "'); ";
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();

                exept = "\n Data added to Speciality table\n";
                return true;
            }
            catch (Exception ex)
            {
                exept = "\n" + ex.Message + "\n";
                return false;
            }
        }

        public bool AddDataToTableWorker(int id, string name, int? bossId, int SpecialityId, int IsBoss, out string exept)
        {
            try
            {
                command.CommandText = @"INSERT INTO Worker(id, Name,BossId,SpecialityId,IsBoss) VALUES (" + id + ",'" + name + "'," + (bossId == null ? "'NULL'" : bossId.ToString()) + "," + SpecialityId + "," + IsBoss + "); ";
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();

                exept = "\n Data added to Worker table\n";
                return true;
            }
            catch (Exception ex)
            {
                exept = "\n" + ex.Message + "\n";
                return false;
            }
        }

        public string ShowTableWorkers(out string exept)
        {
            string res = "";
            try
            {
                command.CommandText = "SELECT W.Name AS Name,B.Name AS Boss, S.Name AS Spec " +
                                            "FROM Worker AS W " +
                                            "JOIN Worker AS B " +
                                            "ON B.Id = W.BossId " +
                                            "JOIN Speciality AS S " +
                                            "ON S.Id = W.SpecialityId";

                SQLiteDataReader r = command.ExecuteReader();
                string line = String.Empty;
                res += "Workers:\n";
                while (r.Read())
                {
                    line = r["Name"] + " is a "
                            + r["Spec"] + " and has boss: "
                            + r["Boss"] + ";\n";
                    res += line;
                }
                r.Close();


                exept = "\n Workers showed\n";
                return res;
            }
            catch (Exception ex)
            {
                exept = "\n" + ex.Message + "\n";
                return "";
            }
        }

        public string ShowTableBosses(out string exept)
        {
            string res = "";
            try
            {
                command.CommandText = "SELECT W.Name AS Name, S.Name AS Spec " +
                                            "FROM Worker AS W " +
                                            "JOIN Speciality AS S " +
                                            "ON S.Id = W.SpecialityId " +
                                            "WHERE W.IsBoss = 1";

                SQLiteDataReader r = command.ExecuteReader();
                string line = String.Empty;
                res += "Bosses:\n";
                while (r.Read())
                {
                    line = r["Name"] + " is a "
                         + r["Spec"] + ";\n";
                    Console.WriteLine(line);
                    res += line;
                }
                r.Close();


                exept = "\n Bosses showed\n";
                return res;
            }
            catch (Exception ex)
            {
                exept = "\n" + ex.Message + "\n";
                return "";
            }
        }

        public void Dispose()
        {
            connection.Close();
        }

        public bool ExecuteQuery(string query, string message, out string exept)
        {
            try
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
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




