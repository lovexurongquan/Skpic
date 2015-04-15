using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Skpic.Main.Web.Startup))]
namespace Skpic.Main.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
