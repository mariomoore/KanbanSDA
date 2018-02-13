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
using KanbanSDA.ViewModels;

namespace KanbanSDA.Controllers
{
    public class IssueViewModelController : Controller
    {
        private KanbanContext db = new KanbanContext();

        // GET: IssueViewModel
        //public ActionResult Index()
        //{
        //    var issueViewModels = db.IssueViewModels.Include(i => i.Project);
        //    return View(issueViewModels.ToList());
        //}

        // GET: IssueViewModel/Details/5
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

        // GET: IssueViewModel/Create
        public ActionResult Create(int? projectId)
        {
            if (projectId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //zamiast tego można przekierować do IssueController
            }
            
            var columns = db.Columns.Where(p => p.Board.ProjectId == projectId).ToList();
            var columnsToSelectList = new List<SelectListItem>();
            columnsToSelectList.Add(new SelectListItem() { Text = "BACKLOG", Value = "" });
            foreach (Column col in columns)
            {
                columnsToSelectList.Add(new SelectListItem() { Text = col.Name, Value = col.Id.ToString() });
            }
            
            IssueViewModel issueViewModel = new IssueViewModel();
            issueViewModel.ProjectId = projectId.GetValueOrDefault();
            issueViewModel.ProjectName = db.Projects.Where(p => p.Id == projectId).FirstOrDefault().Name;
            issueViewModel.ColumnsList = columnsToSelectList;

            return View(issueViewModel);
        }

        // POST: IssueViewModel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,ProjectId,ColumnId,Position,CreatedDate,UpdatedDate")] IssueViewModel issueViewModel)
        {
            if (ModelState.IsValid)
            {

                Issue issue = new Issue();
                issue.Id = issueViewModel.Id;
                issue.Name = issueViewModel.Name;
                issue.Description = issueViewModel.Description;
                issue.ProjectId = issueViewModel.ProjectId;
                issue.ColumnId = issueViewModel.ColumnId;
                issue.Position = db.Issues.Where(c => c.ColumnId == issue.ColumnId).Count() + 1;
                issue.CreatedDate = DateTime.Now;
                issue.UpdatedDate = DateTime.Now;
                db.Issues.Add(issue);
                db.SaveChanges();
                return RedirectToAction("Show", "BoardViewModel", new { id = issue.ProjectId });
            }

            return View(issueViewModel);
        }

        // GET: IssueViewModel/Edit/5
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

            var projects = db.Projects.Select(p => p).ToList();
            var projectsToSelectList = new List<SelectListItem>();
            projectsToSelectList.Add(new SelectListItem() { Text = "---", Value = "" });
            foreach (Project proj in projects)
            {
                projectsToSelectList.Add(new SelectListItem() { Text = proj.Name, Value = proj.Id.ToString() });
            }

            var columns = db.Columns.Where(p => p.Board.ProjectId == issue.ProjectId).ToList();
            var columnsToSelectList = new List<SelectListItem>();
            columnsToSelectList.Add(new SelectListItem() { Text = "BACKLOG", Value = "" });
            foreach (Column col in columns)
            {
                columnsToSelectList.Add(new SelectListItem() { Text = col.Name, Value = col.Id.ToString() });
            }

            IssueViewModel issueViewModel = new IssueViewModel();
            issueViewModel.Id = issue.Id;
            issueViewModel.Name = issue.Name;
            issueViewModel.Description = issue.Description;
            issueViewModel.ProjectId = issue.ProjectId;
            issueViewModel.ProjectName = projects.Where(p => p.Id == issue.ProjectId).FirstOrDefault().Name;
            issueViewModel.ProjectsList = projectsToSelectList;
            issueViewModel.ColumnId = issue.ColumnId;
            issueViewModel.ColumnsList = columnsToSelectList;
            issueViewModel.Position = issue.Position;
            issueViewModel.CreatedDate = issue.CreatedDate;
            issueViewModel.UpdatedDate = issue.UpdatedDate;

            return View(issueViewModel);
        }

        // POST: IssueViewModel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,ProjectId,ColumnId,Position,CreatedDate,UpdatedDate")] IssueViewModel issueViewModel)
        {
            if (ModelState.IsValid)
            {
                Issue issue = new Issue();

                var oldIssue = db.Issues.Where(p => p.Id == issueViewModel.Id).AsNoTracking().FirstOrDefault();
                if (issueViewModel.ProjectId != oldIssue.ProjectId)
                {
                    issue.ColumnId = null;
                    // trzeba przenumerować pozycje
                }
                else
                {
                    issue.ColumnId = issueViewModel.ColumnId;
                }

                if (issueViewModel.ColumnId != oldIssue.ColumnId)
                {
                    if (issueViewModel.ColumnId == null)
                    {
                        issueViewModel.Position = 0;
                    }
                    else
                    {
                        var issuesQuantity = db.Issues.Where(c => c.ColumnId == issueViewModel.ColumnId).Count();
                        issueViewModel.Position = issuesQuantity + 1;
                    }
                    // trzeba przenumerować pozycje
                }

                issue.Id = issueViewModel.Id;
                issue.Name = issueViewModel.Name;
                issue.Description = issueViewModel.Description;
                issue.ProjectId = issueViewModel.ProjectId;
                issue.Position = issueViewModel.Position;
                issue.CreatedDate = issueViewModel.CreatedDate;
                issue.UpdatedDate = DateTime.Now;
                db.Entry(issue).State = EntityState.Modified;
                db.SaveChanges();

                ResetIssuesPosition(oldIssue.ColumnId.GetValueOrDefault());

                if (issue.ProjectId == null)
                {
                    return RedirectToAction("Index", "Issue");
                }
                
                return RedirectToAction("Show", "BoardViewModel", new { id = issue.ProjectId });
            }

            return View(issueViewModel);
        }

        // GET: IssueViewModel/Delete/5
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

        // POST: IssueViewModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Issue issue = db.Issues.Find(id);
            int projId = issue.ProjectId.GetValueOrDefault();
            db.Issues.Remove(issue);
            db.SaveChanges();

            return RedirectToAction("Show", "BoardViewModel", new { id = projId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void ResetIssuesPosition(int columnId)
        {
            var issues = db.Issues.Where(c => c.ColumnId == columnId).ToList();
            int position = 1;
            foreach (Issue iss in issues)
            {
                iss.Position = position;
                db.Entry(iss).State = EntityState.Modified;
                position++;
            }
            db.SaveChanges();
        }
    }
}
