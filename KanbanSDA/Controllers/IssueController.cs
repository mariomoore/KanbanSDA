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
            Issue issue = db.Issues.Find(id);
            if (issue == null)
            {
                return HttpNotFound();
            }
            return View(issue);
        }

        // GET: Issue/Create
        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");

            return View();
        }

        // POST: Issue/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,ProjectId,ColumnId,Position,CreatedDate,UpdatedDate")] Issue issue)
        {
            if (ModelState.IsValid)
            {
                issue.Position = 0;
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
            ViewBag.ColumnId = new SelectList(db.Columns.Where(p => p.Board.ProjectId == issue.ProjectId), "Id", "Name", issue.ColumnId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", issue.ProjectId);
            ViewBag.CreatedDate = issue.CreatedDate;
            return View(issue);
        }

        // POST: Issue/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,ProjectId,ColumnId,Position,CreatedDate,UpdatedDate")] Issue issue)
        {
            if (ModelState.IsValid)
            {
                var oldIssue = db.Issues.Where(p => p.Id == issue.Id).AsNoTracking().FirstOrDefault();
                if (issue.ProjectId != oldIssue.ProjectId)
                {
                    issue.ColumnId = null;
                }

                if (issue.ColumnId != oldIssue.ColumnId)
                {
                    if (issue.ColumnId == null) // ten warunek nigdy się nie spełni
                    {
                        issue.Position = 0;
                    }
                    else
                    {
                        var issuesQuantity = db.Issues.Where(c => c.ColumnId == issue.ColumnId).Count();
                        issue.Position = issuesQuantity + 1;
                    }
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
            Issue issue = db.Issues.Find(id);
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
