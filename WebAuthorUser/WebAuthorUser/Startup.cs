using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebAuthorUser.Startup))]
namespace WebAuthorUser
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
      
            ConfigureAuth(app);
        }
    }
}
