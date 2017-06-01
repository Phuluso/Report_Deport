using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ReportDeport.Startup))]
namespace ReportDeport
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
