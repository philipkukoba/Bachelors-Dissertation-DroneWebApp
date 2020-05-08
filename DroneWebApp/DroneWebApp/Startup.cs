using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DroneWebApp.Startup))]
namespace DroneWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
