using MO_Erasoft.Models;
using System.Data.Entity;

namespace MO_Erasoft
{
    public class SIDbContext : DbContext
    {
        public virtual DbSet<SIT01A> SIT01A { get; set; }

        public virtual DbSet<SIT01B> SIT01B { get; set; }


        public SIDbContext() : base("name=SIDbContext") { }
        public SIDbContext(string dbSourceSI) : base("name=SIDbContext") { }

        public SIDbContext(string dbSourceEra, string dbPathEra)
           : base($"Server={dbSourceEra};initial catalog={dbPathEra};" +
                  $"user id=sa;password=^^^666;multipleactiveresultsets=True;" +
                  $"application name=EntityFramework")
        {
        }

    }
}