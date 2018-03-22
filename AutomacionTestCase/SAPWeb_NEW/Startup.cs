using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SAPWeb_NEW.Startup))]
namespace SAPWeb_NEW
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
