using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KanbanSDA.DAL;
using KanbanSDA.Models;

namespace KanbanSDA.Controllers
{
    public class IssueController : Controller
    {
        private KanbanContext db = new KanbanContext();

        // GET: Issue
        public ActionResult Index()
        {
            var issues = db.Issues.Include(i => i.Column).Include(i => i.Project);
            return View(issues.ToList());
        }

        // GET: Issue/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Issue issue = db.Issues.Find(id);
            Issue issue = db.Issues.Where(i => i.Id==id).Include(i => i.Project).FirstOrDefault();
            if (issue == null)
            {
                return HttpNotFound();
            }
            return View(issue);
        }

        // GET: Issue/Create
        public ActionResult Create()
        {
            ViewBag.ColumnId = new SelectList(db.Columns, "Id", "Name");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            return View();
        }

        // POST: Issue/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,ProjectId,ColumnId,CreatedDate,UpdatedDate")] Issue issue)
        {
            if (ModelState.IsValid)
            {
                issue.CreatedDate = DateTime.UtcNow;
                issue.UpdatedDate = DateTime.UtcNow;
                db.Issues.Add(issue);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ColumnId = new SelectList(db.Columns, "Id", "Name", issue.ColumnId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", issue.ProjectId);
            return View(issue);
        }

        // GET: Issue/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Issue issue = db.Issues.Find(id);
            if (issue == null)
            {
                return HttpNotFound();
            }
            ViewBag.ColumnId = new SelectList(db.Columns, "Id", "Name", issue.ColumnId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", issue.ProjectId);
            return View(issue);
        }

        // POST: Issue/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,ProjectId,ColumnId,CreatedDate,UpdatedDate")] Issue issue)
        {
            if (ModelState.IsValid)
            {
                var projectId = db.Issues.Where(i => i.Id == issue.Id).Select(i => i.ProjectId).FirstOrDefault();
                if(issue.ProjectId != projectId)
                {
                    issue.ColumnId = null;
                }
                issue.UpdatedDate = DateTime.UtcNow;
                db.Entry(issue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ColumnId = new SelectList(db.Columns, "Id", "Name", issue.ColumnId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", issue.ProjectId);
            return View(issue);
        }

        // GET: Issue/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Issue issue = db.Issues.Where(i => i.Id == id).Include(i => i.Project).FirstOrDefault();
            if (issue == null)
            {
                return HttpNotFound();
            }
            return View(issue);
        }

        // POST: Issue/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Issue issue = db.Issues.Find(id);
            db.Issues.Remove(issue);
            db.SaveChanges();
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
