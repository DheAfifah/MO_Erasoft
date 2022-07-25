using MO_Erasoft.Models;
using System.Data.Entity;

namespace MO_Erasoft
{
    public class ARDbContext : DbContext
    {
        public virtual DbSet<ARF01> ARF01 { get; set; }

        public ARDbContext() : base("name=ARDbContext") { }
        public ARDbContext(string dbSourceAR) : base("name=ARDbContext") { }

        public ARDbContext(string dbSourceEra, string dbPathEra)
           : base($"Server={dbSourceEra};initial catalog={dbPathEra};" +
                  $"user id=sa;password=^^^666;multipleactiveresultsets=True;" +
                  $"application name=EntityFramework")
        {
        }
    }
}