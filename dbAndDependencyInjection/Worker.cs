namespace dbAndDependencyInjection
{
    public class Worker
    {
        public int id { get; set; }
        public string Name { get; set; }
        public int? BossId { get; set; }
        public int SpecialityId { get; set; }
        public int IsBoss { get; set; }
       
    }
    public class Speciality
    {
        public int id { get; set; }
        public string Name { get; set; }

    }
    public class WorkerNotBoss
    {
        public string Name { get; set; }
        public string Boss { get; set; }
        public string Speciality { get; set; }

    }

    public class WorkerIsBoss
    {
        public string Name { get; set; }
        public string Boss { get; set; }
        public string Speciality { get; set; } 

    }
}
