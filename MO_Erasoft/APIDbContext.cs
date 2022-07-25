using MO_Erasoft.Models;
using System.Data.Entity;

namespace MO_Erasoft
{
    public class APIDbContext : DbContext
    {
        public APIDbContext() : base("name=APIDbContext") { }
        public APIDbContext(string dbSourceAPI) : base("name=APIDbContext") { }

        public APIDbContext(string dbSourceEra, string dbPathEra)
           : base($"Server={dbSourceEra};initial catalog={dbPathEra};" +
                  $"user id=sa;password=^^^666;multipleactiveresultsets=True;" +
                  $"application name=EntityFramework")
        {
        }
    }
}