using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace PJMAN1_DEMO_.SignalR.hubs
{
    public class CHub : Hub
    { 
        public override System.Threading.Tasks.Task OnConnected()
        {
            Clients.All.user(Context.User.Identity.Name);
            return base.OnConnected();
        }

        public void send(string message)
        {
            Clients.Caller.message("You~You:" + message);
            Clients.Others.message("others~" + Context.User.Identity.Name + ":" + message);
        }
       /*public void Send(string name, string message)
        {
            Clients.All.addNewMessageToPage(name, message);
        }*/
    }
}