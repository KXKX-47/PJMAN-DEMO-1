using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PJMAN1_DEMO_.Models;
using System.Security.Cryptography;
using System.Text;

namespace PJMAN1_DEMO_.Controllers
{
    public class MainController : Controller
    {
        private DB_Entities _db = new DB_Entities();
        public ActionResult MainPage()
        {
            if (Session["Id"] != null)
            {
                return View("Login");
            }
            else
            {
                return RedirectToAction("Login");
            }
           
        }


        public ActionResult Register()
        {
            //ViewBag.Message = "Your contact page.";

            return View();
        }

        //POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User _user)
        {
            if (ModelState.IsValid)
            {
                var check = _db.PjTable.FirstOrDefault(s => s.Email == _user.Email);
                if (check == null)
                {
                    _user.Password = GetMD5(_user.Password);
                    _db.Configuration.ValidateOnSaveEnabled = false;
                    _db.PjTable.Add(_user);
                    _db.SaveChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }
            }
            return View();
        }

        public ActionResult Login()
        {
           // ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email , string password)
        {
            string message = "";
            if (ModelState.IsValid)
            {
                var f_password = GetMD5(password);
                var data = _db.PjTable.Where(s => s.Email.Equals(email) && s.Password.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["ProjectCode"] = data.FirstOrDefault().CompanyName + " " + data.FirstOrDefault().ProjectName;
                    Session["Email"] = data.FirstOrDefault().Email;
                    Session["Id"] = data.FirstOrDefault().Id;
                    return RedirectToAction("HomePage");
                }
                else
                {
                    message = "Login failed , Register or Try Again";
                    TempData["Error Message"] = message;
                    ViewBag.error = "Login failed";
                    return RedirectToAction("Login");
                }

                
            }
            return View();
        }

        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }

        public ActionResult HomePage()
        {
            return View();
        }

        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }

            return byte2String;
        }

    }
}