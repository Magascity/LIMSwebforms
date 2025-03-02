using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LIMSwebforms.Startup))]
namespace LIMSwebforms
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
