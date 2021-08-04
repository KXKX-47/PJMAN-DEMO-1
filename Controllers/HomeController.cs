using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using PJMAN1_DEMO_.Models;

namespace PJMAN1_DEMO_.Controllers
{
    public class calendarEvent //try this method for task list
    {
        public string Summary { get; set; }
        public string Organizer { get; set; }
        public string Description { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set;  }
        public string ColorId { get; set; }
    }
    public class HomeController : Controller
    {
        //public List<string> GoogleEvents = new List<string>();
        public List<calendarEvent> GoogleEvents = new List<calendarEvent>();
        static string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        static string ApplicationName = "Google Calendar API .NET Quickstart";
        // GET: Home

        /*static IList<Tasks> taskList = new List<Tasks>
        {
            new Tasks() {TID = 1, Task = "Task 1"} ,
            new Tasks() {TID = 2,Task = "Task 2"}
        };

        static IList<BTasks> PtaskList = new List<BTasks>
        {

        };

        static IList<Tasks> CTaskList = new List<Tasks>
        {
            new Tasks() {TID = 4, Task = "Task 4"},
            new Tasks() {TID = 5, Task = "Task 5"}
        }; */







        public ActionResult HomePage()
        {
            

            
            return View();
        }

        

        public ActionResult Calendar()
        {
            CalendarEvents();
            ViewBag.EventList = GoogleEvents;
            return View();
        }

        public void CalendarEvents()
        {
            UserCredential credential;
            string path = Server.MapPath("/Credentials.json");
            

            using (var stream =
                new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    //GoogleClientSecrets.Load(stream).Secrets,
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                // Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define parameters of request.
            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // List events.
            Events events = request.Execute();
            Console.WriteLine("Upcoming events:");
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    var calendarevent = new calendarEvent();
                    calendarevent.Organizer = eventItem.Organizer.Email;
                    calendarevent.Summary = eventItem.Summary;
                    calendarevent.StartTime = eventItem.Start.DateTime.ToString();
                    calendarevent.EndTime = eventItem.End.DateTime.ToString();
                    calendarevent.Description = eventItem.Description;
                    calendarevent.ColorId = eventItem.ColorId;
                    GoogleEvents.Add(calendarevent);
                    //GoogleEvents.Add(eventItem.Summary);
                    /* string when = eventItem.Start.DateTime.ToString();
                     if (String.IsNullOrEmpty(when))
                     {
                         when = eventItem.Start.Date;
                     }
                     Console.WriteLine("{0} ({1})", eventItem.Summary, when);*/
                }
            }
            /*else
            {
                Console.WriteLine("No upcoming events found.");
            }*/
        }

        public JsonResult GetEvents()
        {
            using (CalendarDatabaseEntities dc = new CalendarDatabaseEntities())
            {
                var events = dc.Tables.ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public ActionResult Task()
        {
            var tasks = taskList();
            var ptasks = ptaskList();
            var ctasks = ctaskList();

            ViewModel model = new ViewModel();
            model.Tasks = tasks;
            model.BTasks = ptasks;
            model.CTasks = ctasks;

            return View(model);
            
        }

        private List<Tasks> taskList()
        {
            return new List<Tasks>()
            {
                new Tasks() {TID = 1, Task = "Task 1"} ,
                new Tasks() {TID = 2,Task = "Task 2"}
            };
        }

        private List<BTasks> ptaskList()
        {
            return new List<BTasks>()
            {
                new BTasks() {TID = 3, Task = "Task 3"} ,
                new BTasks() {TID = 4,Task = "Task 4"}
            };
        }

        private List<CTasks> ctaskList()
        {
            return new List<CTasks>()
            {
                new CTasks() {TID = 5 , Task = "Task 5"},
                new CTasks() {TID = 6, Task = "Task 6"}
            };
        }

        public ActionResult ChatSystem()
        {
            return View("ChatSystem");
        }

        public ActionResult TaskEdit(int Id)
        {
            var tasks = taskList();
            var ptasks = ptaskList();
            var ctasks = ctaskList();

            ViewModel model = new ViewModel();
            model.Tasks = tasks;
            model.BTasks = ptasks;
            model.CTasks = ctasks;

            var tsk = tasks.Where(s => s.TID == Id).FirstOrDefault();
            return View(tsk);
        }

        [HttpPost]
        public ActionResult TaskEdit(Tasks tsk)
        {
            var tasks = taskList();
            var ptasks = ptaskList();
            var ctasks = ctaskList();

            ViewModel model = new ViewModel();
            model.Tasks = tasks;
            model.BTasks = ptasks;
            model.CTasks = ctasks;
            var task = model.Tasks.Where(s => s.TID == tsk.TID).FirstOrDefault();

            //taskList.Remove(task);
            //taskList.Add(tsk);
            model.Tasks.Remove(task);
            model.Tasks.Add(tsk);



            return RedirectToAction("Task");
        }

       
    }
}