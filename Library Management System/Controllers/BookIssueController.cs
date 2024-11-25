using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DatabaseLayer;
using ClosedXML;
using DocumentFormat.OpenXml.Drawing.ChartDrawing;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Library_Management_System.Controllers
{
    public class BookIssueController : Controller
    {
        private CMPK_LMSEntities db = new CMPK_LMSEntities();

        // GET: BookIssue
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var bookIssueTables = db.BookIssueTables.Include(b => b.BooksTable).Include(b => b.EmployeeHRM);
            return View(bookIssueTables.ToList());

        }


        // GET: Return Pending
        public ActionResult ReturnPending()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var bookIssueTables = db.BookIssueTables.Where(b => b.Status == false).Include(b => b.BooksTable).Include(b => b.EmployeeHRM);
            return View(bookIssueTables.ToList());
        }

        // GET: Delayed Books
        public ActionResult DelayedBooks()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var bookIssueTables = db.BookIssueTables.Where(b => b.ReturnDate < DateTime.Now && b.Status==false).Include(b => b.BooksTable).Include(b => b.EmployeeHRM);
            return View(bookIssueTables.ToList());
        }

        // GET: Return Pending
        public ActionResult ReturnHistory()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var bookIssueTables = db.BookIssueTables.Include(b => b.BooksTable).Include(b => b.EmployeeHRM).Where(b => b.Status == true);
            return View(bookIssueTables.ToList());
        }
        // GET: BookIssue/IssueDetails/5
        public ActionResult IssueDetails(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //BookIssueTable bookIssueTable = db.BookIssueTables.Find(id);
            //if (bookIssueTable == null)
            //{
            //    return HttpNotFound();
            //}
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var bookIssueTables = db.BookIssueTables.Include(b => b.BooksTable).Include(b => b.EmployeeHRM).Where(b => b.IBID == id);
            return View(bookIssueTables.ToList());

            //return View(bookIssueTable);
        }

        // GET: BookIssue/CollectDetails/5
        public ActionResult CollectDetails(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var bookIssueTables = db.BookIssueTables.Include(b => b.BooksTable).Include(b => b.EmployeeHRM).Where(b => b.IBID == id);
            return View(bookIssueTables.ToList());


        }

        // GET: BookIssue/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookIssueTable bookIssueTable = db.BookIssueTables.Find(id);
            if (bookIssueTable == null)
            {
                return HttpNotFound();
            }
            return View(bookIssueTable);
        }

        // GET: BookIssue/Create
        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var booklist = db.BooksTables.Where((b => b.Availability == true)).ToList();
            IEnumerable<SelectListItem> selectList = from b in booklist
                                                     select new SelectListItem
                                                     {
                                                         Value = b.BookID.ToString(),
                                                         Text = b.BookID + " - " + b.BookName
                                                     };
            ViewBag.BookID = new SelectList(selectList, "Value", "Text");
            ViewBag.EMP_EPFNo = new SelectList(db.EmployeeHRMS, "EMP_EPFNo", "EMP_EPFNo");
            return View();

        }

        // POST: BookIssue/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookIssueTable bookIssueTable)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            String username = ((string)Session["UserName"]);
            bookIssueTable.Issueby = username;
            bookIssueTable.IssueDate=DateTime.Now;
            if (ModelState.IsValid)
            {

                var SBookid = db.BooksTables.Find(bookIssueTable.BookID);
                SBookid.Availability = false;
                db.BookIssueTables.Add(bookIssueTable);
                db.SaveChanges();
                db.Entry(SBookid).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

            }

            var booklist = db.BooksTables.Where((b => b.Availability == true)).ToList();
            IEnumerable<SelectListItem> selectList = from b in booklist
                                                     select new SelectListItem
                                                     {
                                                         Value = b.BookID.ToString(),
                                                         Text = b.BookID + " - " + b.BookName
                                                     };
            ViewBag.BookID = new SelectList(selectList, "Value", "Text", bookIssueTable.BookID);
            ViewBag.EMP_EPFNo = new SelectList(db.EmployeeHRMS, "EMP_EPFNo", "EMP_EPFNo", bookIssueTable.EMP_EPFNo);
            return View(bookIssueTable);

        }

        public ActionResult Collect(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookIssueTable findbook = db.BookIssueTables.Find(id);
            if (findbook == null)
            {
                return HttpNotFound();
            }
            var SBookid = db.BooksTables.Find(findbook.BookID);
            SBookid.Availability = true;

            String username = ((string)Session["UserName"]);
            findbook.Collectby = username;
            findbook.CollectDate = DateTime.Now;
            findbook.Status = true;
            TempData["Message"] = "( "+findbook.BooksTable.BookName+" )" + "has been Collected";
            db.Entry(findbook).State = EntityState.Modified;
            db.SaveChanges();
            db.Entry(SBookid).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ReturnPending");
           
        }


        // GET: BookIssue/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
        //    {
        //        return RedirectToAction("Login", "Home");
        //    }
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    BookIssueTable bookIssueTable = db.BookIssueTables.Find(id);
        //    if (bookIssueTable == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.BookID = new SelectList(db.BooksTables, "BookID", "BookName", bookIssueTable.BookID);
        //    ViewBag.EMP_EPFNo = new SelectList(db.EmployeeHRMS, "EMP_EPFNo", "EID", bookIssueTable.EMP_EPFNo);

        //    return View(bookIssueTable);

        //}

        //// POST: BookIssue/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(BookIssueTable bookIssueTable)
        //{
        //    if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
        //    {
        //        return RedirectToAction("Login", "Home");
        //    }

        //    String username = ((string)Session["UserName"]);
        //    bookIssueTable.Collectby = username;
        //    bookIssueTable.Status = true;
        //    bookIssueTable.CollectDate=DateTime.Now;
        //    bookIssueTable.BookID = bookIssueTable.BookID;
        //    bookIssueTable.Issueby = bookIssueTable.Issueby;
        //    bookIssueTable.IssueDate = bookIssueTable.IssueDate;
        //    bookIssueTable.ReturnDate = bookIssueTable.ReturnDate;
        //    //if (ModelState.IsValid)
        //    //{
                
        //        var SBookid = db.BooksTables.Find(bookIssueTable.BookID);
        //        SBookid.Availability = true;
        //        db.Entry(bookIssueTable).State = System.Data.Entity.EntityState.Modified;
        //        db.SaveChanges();
        //        db.Entry(SBookid).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    //}
        //    //ViewBag.BookID = new SelectList(db.BooksTables, "BookID", "BookID", bookIssueTable.BookID);
        //    //ViewBag.EMP_EPFNo = new SelectList(db.EmployeeHRMS, "EMP_EPFNo", "EMP_EPFNo", bookIssueTable.EMP_EPFNo);

        //    //return View(bookIssueTable);

        //}

        // GET: BookIssue/Delete/5
        public ActionResult Delete(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookIssueTable bookIssueTable = db.BookIssueTables.Find(id);
            if (bookIssueTable == null)
            {
                return HttpNotFound();
            }
            return View(bookIssueTable);
        }

        // POST: BookIssue/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BookIssueTable bookIssueTable = db.BookIssueTables.Find(id);
            db.BookIssueTables.Remove(bookIssueTable);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ExportReturnPending()
        {

            var gv = new GridView();

            gv.DataSource = this.db.ReturnPendings.ToList();
            gv.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Return_Pending_Books.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

            gv.RenderControl(objHtmlTextWriter);

            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("ReturnPending");
        }

        public ActionResult ExportAllIssue() { 

            var gv = new GridView();
            var booklist = db.AllissueBooks.ToList();
            gv.DataSource = booklist;
            gv.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=All_Issued_Books.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

            gv.RenderControl(objHtmlTextWriter);

            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("Index");


        }

        public ActionResult ExportReturned()
        {

            var gv = new GridView();
            var booklist = db.AllissueBooks.ToList();
            gv.DataSource = booklist;
            gv.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=All_Returned_Books.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

            gv.RenderControl(objHtmlTextWriter);

            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("ReturnHistory");


        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
