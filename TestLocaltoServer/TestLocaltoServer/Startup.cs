using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestLocaltoServer.Startup))]
namespace TestLocaltoServer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
