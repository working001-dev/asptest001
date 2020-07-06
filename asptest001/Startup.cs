using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(asptest001.Startup))]
namespace asptest001
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
