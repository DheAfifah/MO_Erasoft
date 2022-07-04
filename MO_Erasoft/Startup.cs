using Owin;
using Microsoft.Owin;
[assembly: OwinStartup(typeof(MO_Erasoft.Startup))]

namespace MO_Erasoft
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
            //GlobalConfiguration.Configuration
            //    //.UseSqlServerStorage(@"Data Source=DESKTOP-CKMIJJC\SQLEXPRESS;Initial Catalog=hangfire_risa;Integrated Security=True;Pooling=False")
            //    //.UseSqlServerStorage(@"Data Source=18.138.225.65\SQLEXPRESS, 1433;Initial Catalog=hangfire_risa;user id=sa;password=risacorps135_;MultipleActiveResultSets=True;App=EntityFramework")
            //    .UseSqlServerStorage(@"Data Source=172.31.13.214\SQLEXPRESS, 1433;Initial Catalog=hangfire_risa;user id=sa;password=risacorps135_;MultipleActiveResultSets=True;App=EntityFramework")
            //    .UseRecurringJobAdmin(typeof(Startup).Assembly);


            //app.UseHangfireDashboard();
            //app.UseHangfireServer();
        }
    }
}