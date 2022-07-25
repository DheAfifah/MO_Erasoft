using MO_Erasoft.Models;
using System.Data.Entity;

namespace MO_Erasoft
{
    public class STDbContext : DbContext
    {
        public STDbContext() : base("name=STDbContext") { }
        public STDbContext(string dbSourceSI) : base("name=STDbContext") { }

        public STDbContext(string dbSourceEra, string dbPathEra)
           : base($"Server={dbSourceEra};initial catalog={dbPathEra};" +
                  $"user id=sa;password=^^^666;multipleactiveresultsets=True;" +
                  $"application name=EntityFramework")
        {
        }
    }
}