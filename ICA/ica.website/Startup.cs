using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ica.website.Startup))]
namespace ica.website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
