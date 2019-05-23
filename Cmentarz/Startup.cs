using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Cmentarz.Startup))]
namespace Cmentarz
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
