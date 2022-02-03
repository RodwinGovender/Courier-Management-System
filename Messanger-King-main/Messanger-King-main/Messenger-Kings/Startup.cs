using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Messenger_Kings.Startup))]
namespace Messenger_Kings
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
