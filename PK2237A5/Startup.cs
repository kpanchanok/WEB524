using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PK2237A5.Startup))]

namespace PK2237A5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
