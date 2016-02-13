using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BudgetYou.Startup))]
namespace BudgetYou
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
