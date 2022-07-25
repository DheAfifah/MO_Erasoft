using MO_Erasoft.Models;
using System.Data.Entity;

namespace MO_Erasoft
{
    public class AKUN_ERA_MODbContext : DbContext
    {
        public AKUN_ERA_MODbContext() : base("name=AKUN_ERA_MODbContext") { }
        public AKUN_ERA_MODbContext(string dbSourceRisa) : base("name=AKUN_ERA_MODbContext") { }
    }
}