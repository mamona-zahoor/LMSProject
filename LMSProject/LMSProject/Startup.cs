using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LMSProject.Startup))]
namespace LMSProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
