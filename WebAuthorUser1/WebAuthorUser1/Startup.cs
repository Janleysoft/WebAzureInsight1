using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebAuthorUser1.Startup))]
namespace WebAuthorUser1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
