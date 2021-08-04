using Microsoft.Owin;
using Owin;
[assembly: OwinStartup(typeof(PJMAN1_DEMO_.StartUp))]

namespace PJMAN1_DEMO_
{
    public class StartUp
    {
        public void Configuration(IAppBuilder app)
        {
            // any connection or hub wire should go here
            app.MapSignalR();
        }

    }
}