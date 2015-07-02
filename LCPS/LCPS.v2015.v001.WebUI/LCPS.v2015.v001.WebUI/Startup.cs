using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LCPS.v2015.v001.WebUI.Startup))]
namespace LCPS.v2015.v001.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
