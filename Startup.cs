using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Buoi_4_Bai_BigSchool__.Startup))]
namespace Buoi_4_Bai_BigSchool__
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
