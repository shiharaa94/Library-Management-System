using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.DirectoryServices.AccountManagement;

namespace Library_Management_System.Controllers
{
    public class HomeController : Controller
    {
        private CMPK_LMSEntities db = new CMPK_LMSEntities();

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginUser(string username, string password)
        {
            try
            {
                if (username != null && password != null)
                {

                    Boolean isvalid;
                    PrincipalContext pc = new PrincipalContext(ContextType.Domain, "SL");
                    isvalid = pc.ValidateCredentials(username, password);
                    if(isvalid)
                    {
                        var finduser = db.UserTables.Where(u => u.UserName == username).ToList();
                    if (finduser.Count() == 1)
                        {
                            Session["UserID"] = finduser[0].UserID;
                            Session["UserTypeID"] = finduser[0].UserTypeID;
                            Session["UserName"] = finduser[0].UserName;
                        
                        

                            string url = string.Empty;
                            if (finduser[0].UserTypeID == 1)
                            {
                                url = "Dashboard";
                            }
                            else if (finduser[0].UserTypeID == 2)
                            {
                                return RedirectToAction("Dashboard");
                            }
                            else if (finduser[0].UserTypeID == 4)
                            {
                                 return RedirectToAction("Dashboard");
                            }
                            else
                            {
                                url = "Dashboard";
                            }
                            return RedirectToAction(url);

                        }
                            else
                            {
                                Session["UserID"] = string.Empty;
                                Session["UserTypeID"] = string.Empty;
                                Session["UserName"] = string.Empty;
                                Session["Password"] = string.Empty;
                                ViewBag.message = "UserName Or Password is Incorrect !";
                            }
                    }
                    else if(username=="LSO" || username == "lso")
                        {
                            var finduser = db.UserTables.Where(u => u.UserName == username && u.Password==password).ToList();
                            if (finduser.Count() == 1)
                            {
                                Session["UserID"] = finduser[0].UserID;
                                Session["UserTypeID"] = finduser[0].UserTypeID;
                                Session["UserName"] = finduser[0].UserName;



                                    string url = string.Empty;
                                if (finduser[0].UserTypeID == 1)
                                {
                                    url = "Dashboard";
                                }
                                else if (finduser[0].UserTypeID == 2)
                                {
                                    return RedirectToAction("Dashboard");
                                }
                                else if (finduser[0].UserTypeID == 4)
                                {
                                    return RedirectToAction("Dashboard");
                                }
                            
                                else
                                {
                                    url = "Dashboard";
                                }
                                return RedirectToAction(url);

                            }
                        else
                        {
                            Session["UserID"] = string.Empty;
                            Session["UserTypeID"] = string.Empty;
                            Session["UserName"] = string.Empty;
                            Session["Password"] = string.Empty;
                            ViewBag.message = "Password is Incorrect !";
                        }

                    }

                    //var finduser = db.UserTables.Where(u => u.UserName == username && u.Password == password).ToList();
                    //if (finduser.Count() == 1)
                    //{
                    //    Session["UserID"] = finduser[0].UserID;
                    //    Session["UserTypeID"] = finduser[0].UserTypeID;
                    //    Session["UserName"] = finduser[0].UserName;
                    //    Session["Password"] = finduser[0].Password;


                    //    string url = string.Empty;
                    //    if (finduser[0].UserTypeID == 1)
                    //    {
                    //        url = "Dashboard";
                    //    }
                    //    else if (finduser[0].UserTypeID == 2)
                    //    {
                    //        return RedirectToAction("Dashboard");
                    //    }
                    //    else if (finduser[0].UserTypeID == 4)
                    //    {
                    //        return RedirectToAction("Dashboard");
                    //    }
                    //    else
                    //    {
                    //        url = "Dashboard";
                    //    }
                    //    return RedirectToAction(url);

                    //}
                    //else
                    //{
                    //    Session["UserID"] = string.Empty;
                    //    Session["UserTypeID"] = string.Empty;
                    //    Session["UserName"] = string.Empty;
                    //    Session["Password"] = string.Empty;
                    //    ViewBag.message = "UserName Or Password is Incorrect !";
                    //}
                }
                else
                {
                    Session["UserID"] = string.Empty;
                    Session["UserTypeID"] = string.Empty;
                    Session["UserName"] = string.Empty;
                    Session["Password"] = string.Empty;
                   
                    ViewBag.message = "Some Unexpected Issue is occure. Please Try again !";
                }
            }
            catch (Exception ex)
            {
                Session["UserID"] = string.Empty;
                Session["UserTypeID"] = string.Empty;
                Session["UserName"] = string.Empty;
                Session["Password"] = string.Empty;
               
                ViewBag.message = ex.Message;

            }

            return View("Login");

        }

        public ActionResult Logout()
        {
            Session["UserID"] = string.Empty;
            Session["UserTypeID"] = string.Empty;
            Session["UserName"] = string.Empty;
            Session["Password"] = string.Empty;
            return View("Login");
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            // Total Books
            int totalbooks = db.BooksTables.Count();
            if (totalbooks != 0)
            {
                
                ViewBag.totalbooks = totalbooks;
            }
            else
            {
                ViewBag.totalbooks = 0;
            }

            // Onhand Books
            int onhandbooks = db.BooksTables.Where(b=>b.Availability==true).Count();
            if (onhandbooks != 0)
            {

                ViewBag.onhandbooks = onhandbooks;
            }
            else
            {
                ViewBag.onhandbooks = 0;
            }
            // Return Pending Books
            int pending =db.BookIssueTables.Where(b=>b.Status==false).Count();
            if(pending > 0)
            {
                ViewBag.pending=pending;
            }
            else
            {
                ViewBag.pending=0;
            }
            //Prasantage of Books Userd

            if (pending > 0)
            {
                int presantage = (pending * 100) / totalbooks;
                ViewBag.presantage=presantage;
            }
            else
            {
                ViewBag.presantage=0;
            }
            //Delayed Book
            int delayedbooks = db.BookIssueTables.Where(b => b.ReturnDate < DateTime.Now && b.Status==false).Count();
            if (delayedbooks > 0)
            {
                ViewBag.delayedbooks = delayedbooks;
            }
            else
            {
                ViewBag.delayedbooks = 0;
            }

            // Category wise Books Details......................................
            int childrens = db.BooksTables.Where(b => b.BookTypeID==3).Count();
            if (childrens > 0)
            {
                ViewBag.childrens = childrens;
            }
            else
            {
                ViewBag.childrens = 0;
            }

            int education = db.BooksTables.Where(b => b.BookTypeID == 4).Count();
            if (childrens > 0)
            {
                ViewBag.education = education;
            }
            else
            {
                ViewBag.education = 0;
            }

            int e_childrens = db.BooksTables.Where(b => b.BookTypeID == 5).Count();
            if (e_childrens > 0)
            {
                ViewBag.e_childrens = e_childrens;
            }
            else
            {
                ViewBag.e_childrens = 0;
            }

            int miscellaneous = db.BooksTables.Where(b => b.BookTypeID == 6).Count();
            if (miscellaneous > 0)
            {
                ViewBag.miscellaneous = miscellaneous;
            }
            else
            {
                ViewBag.miscellaneous = 0;
            }

            int novels = db.BooksTables.Where(b => b.BookTypeID == 7).Count();
            if (miscellaneous > 0)
            {
                ViewBag.novels = novels;
            }
            else
            {
                ViewBag.novels = 0;
            }

            int stories = db.BooksTables.Where(b => b.BookTypeID == 8).Count();
            if (stories > 0)
            {
                ViewBag.stories = stories;
            }
            else
            {
                ViewBag.stories = 0;
            }
            
            int Tamil_Miscellaneous = db.BooksTables.Where(b => b.BookTypeID == 9).Count();
            if (Tamil_Miscellaneous > 0)
            {
                ViewBag.Tamil_Miscellaneous = Tamil_Miscellaneous;
            }
            else
            {
                ViewBag.Tamil_Miscellaneous = 0;
            }

            int technology = db.BooksTables.Where(b => b.BookTypeID == 10).Count();
            if (technology > 0)
            {
                ViewBag.technology = technology;
            }
            else
            {
                ViewBag.technology = 0;
            }
            int positive = db.BooksTables.Where(b => b.BookTypeID == 11).Count();
            if (positive > 0)
            {
                ViewBag.positive = positive;
            }
            else
            {
                ViewBag.positive = 0;
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}