using Autofac;
using System;

namespace AutofacExample
{

    class Program
    {
        static void Main(string[] args)
        {
            string str = @"Data Source=..\..\..\DB.db";
            string exeptions = "";
            Console.WriteLine("1 - ADO.NET");
            Console.WriteLine("2 - Dapper");
            Console.WriteLine("3 - Entity Framework");
            IContainer container;
            switch (Console.ReadLine())
            {
                case "1":
                    container = BuildContainer<dbAndDependencyInjection.ADOsqlConnector>();
                    break;
                case "2":
                    container = BuildContainer<dbAndDependencyInjection.DapperSQLConnector>();
                    break;
                case "3":
                    container = BuildContainer<dbAndDependencyInjection.EntityFrameworkConnector>();
                    break;
                default:
                    container = BuildContainer<dbAndDependencyInjection.DapperSQLConnector>();
                    break;
            }
            
            using (var dbConnector = container.Resolve<dbAndDependencyInjection.IsqlConnector>())
            {
                dbConnector.SQLConnect(str, out exeptions);
                Console.WriteLine(exeptions);
                Console.WriteLine(exeptions);
                dbConnector.CreateTableWorker(out exeptions);//don't work with Entity Framework
                Console.WriteLine(exeptions);
                dbConnector.CreateTableSpeciality(out exeptions);//don't work with Entity Framework
                Console.WriteLine(exeptions);
                dbConnector.AddDataToTableSpeciality(0, "chief medical officer", out exeptions);
                Console.WriteLine(exeptions);
                dbConnector.AddDataToTableSpeciality(1, "doctor", out exeptions);
                Console.WriteLine(exeptions);
                dbConnector.AddDataToTableWorker(0, "Bob", null, 0, 1, out exeptions);
                Console.WriteLine(exeptions);
                dbConnector.AddDataToTableWorker(1, "John", 0, 1, 0, out exeptions);
                Console.WriteLine(exeptions);
                dbConnector.AddDataToTableWorker(2, "Eliot", 0, 1, 0, out exeptions);
                Console.WriteLine(exeptions);
                Console.WriteLine(dbConnector.ShowTableWorkers(out exeptions));
                Console.WriteLine(dbConnector.ShowTableBosses(out exeptions));
            }

            Console.ReadLine();
        }

        static IContainer BuildContainer<T>()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<T>()
                   .As<dbAndDependencyInjection.IsqlConnector>()
                   .InstancePerDependency();
            return builder.Build();
        }
    }
}