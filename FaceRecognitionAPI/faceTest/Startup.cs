using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(faceTest.Startup))]
namespace faceTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
