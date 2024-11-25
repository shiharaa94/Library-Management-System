using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.UI;
using DatabaseLayer;
using ClosedXML;
using DocumentFormat.OpenXml.Drawing.ChartDrawing;

namespace Library_Management_System.Controllers
{
    public class BooksController : Controller
    {
        private CMPK_LMSEntities db = new CMPK_LMSEntities();

        // GET: Books
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var booksTables = db.BooksTables.Include(b => b.BookTypesTable).Include(b => b.UserTable);
            return View(booksTables.ToList());
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            var booksTables = db.BooksTables.Include(b => b.BookTypesTable).Include(b => b.UserTable).Where(b=>b.BookID==id);
            return View(booksTables.ToList());
            
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int bookid = 1 + (db.BooksTables.Max(b => b.BookID));
            ViewBag.BookID = bookid;
            ViewBag.BookTypeID = new SelectList(db.BookTypesTables, "BookTypeID", "BookType");
            return View();
        }
        
        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BooksTable booksTable)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            int bookid = 1 + (db.BooksTables.Max(b => b.BookID));
            booksTable.UserID = userid;
            booksTable.BookID = bookid;
            if (ModelState.IsValid)
            {
                db.BooksTables.Add(booksTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BookTypeID = new SelectList(db.BookTypesTables, "BookTypeID", "BookType", booksTable.BookTypeID);
            return View(booksTable);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BooksTable booksTable = db.BooksTables.Find(id);
            if (booksTable == null)
            {
                return HttpNotFound();
            }
            ViewBag.BookTypeID = new SelectList(db.BookTypesTables, "BookTypeID", "BookType", booksTable.BookTypeID);
            return View(booksTable);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( BooksTable booksTable)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            booksTable.UserID = userid;

            if (ModelState.IsValid)
            {
                db.Entry(booksTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BookTypeID = new SelectList(db.BookTypesTables, "BookTypeID", "BookType", booksTable.BookTypeID);
            return View(booksTable);
        }

        public ActionResult ExportDataToExcel()
        {

            var gv = new GridView();
            var booklist = db.Booksviews.ToList();
            gv.DataSource = booklist;
            gv.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=AllBooks.xls");
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
