using System;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace dbAndDependencyInjection
{

    public class EntityFrameworkContext : DbContext
    {
        public EntityFrameworkContext()
              : base("DefaultConnection")
        {
            Console.WriteLine("Connect");
        }
        public DbSet<Worker> Worker { get; set; }
        public DbSet<Speciality> Speciality { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }


    public class EntityFrameworkConnector : IsqlConnector
    {
        private EntityFrameworkContext _dbContext;
        

        public bool AddDataToTableSpeciality(int id, string name, out string exept)
        {
            try
            {
                _dbContext.Speciality.Add(new Speciality() { id = id, Name = name });
                _dbContext.SaveChanges();
                exept = "Data was added";
                return true;
            }
            catch (Exception ex)
            {
                exept = ex.Message;
                return false;
            }
        }

        public bool AddDataToTableSpeciality(string name, out string exept)
        {
            try
            {
                _dbContext.Speciality.Add(new Speciality() { id = 0, Name = name });
                _dbContext.SaveChanges();
                exept = "Data was added";
                return true;
            }
            catch (Exception ex)
            {
                exept = ex.Message;
                return false;
            }
        }

        public bool AddDataToTableWorker(int id, string name, int? bossId, int specialityId, int isBoss, out string exept)
        {
            try
            {
                _dbContext.Worker.Add(new Worker() { id = id, Name = name, SpecialityId = specialityId, BossId = bossId, IsBoss = isBoss });
                _dbContext.SaveChanges();
                exept = "Data was added";
                return true;
            }
            catch (Exception ex)
            {
                exept = ex.Message;
                return false;
            }
        }

        //dont work
        public bool CreateTableSpeciality(out string exept)
        {
            throw new NotImplementedException();
        }

        //dont work
        public bool CreateTableWorker(out string exept)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        //dont work
        public bool ExecuteQuery(string query, string message, out string exept)
        {
            try
            {
                _dbContext.Worker.Add(new Worker() { id = 1, Name = "Stew", SpecialityId = 1, BossId = 2, IsBoss = 0 });
                _dbContext.SaveChanges();
                exept = "Data was added";
                return true;
            }
            catch(Exception ex)
            {
                exept = ex.Message;
                return false;
            }       
        }

        public string ShowTableBosses(out string exept)
        {
            try
            {
                string res = String.Empty;

                var bosses=from p in _dbContext.Worker
                            join c in _dbContext.Speciality on p.SpecialityId equals c.id
                            where p.IsBoss == 1
                           select new { Name = p.Name, Speciality = c.Name };
                foreach (var p in bosses)
                    res+=$"{p.Name} is {p.Speciality} and boss \n";
                exept = "Data was showed";
                return res;
            }
            catch (Exception ex)
            {
                exept = ex.Message;
                return "";
            }
        }

        public string ShowTableWorkers(out string exept)
        {
            try
            {
                string res = String.Empty;

                var bosses = from p in _dbContext.Worker
                             join c in _dbContext.Speciality on p.SpecialityId equals c.id
                             join b in _dbContext.Worker on p.BossId equals b.id
                             select new { Name = p.Name, Speciality = c.Name, Boss = b.Name };
                foreach (var p in bosses)
                    res += $"{p.Name} is {p.Speciality} and boss has {p.Boss}\n";
                exept = "Data was showed";
                return res;
            }
            catch (Exception ex)
            {
                exept = ex.Message;
                return "";
            }
        }

        public void SQLConnect(string baseName, out string exept)
        {
            try
            {
                _dbContext = new EntityFrameworkContext();
                exept = "We connected";
            }
            catch (Exception ex)
            {
                exept = ex.Message;
            }
        }
    }
}
