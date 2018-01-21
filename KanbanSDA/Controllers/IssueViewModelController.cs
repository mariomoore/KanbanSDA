using KanbanSDA.DAL;
using KanbanSDA.Models;
using KanbanSDA.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace KanbanSDA.Controllers
{
    public class IssueViewModelController : Controller
    {
        private KanbanContext db = new KanbanContext();

        // GET: IssueViewModel
        //public ActionResult Index()
        //{
        //    return View();
        //}

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
            IssueViewModel ivm = new IssueViewModel();
            ivm.Id = issue.Id;
            ivm.Name = issue.Name;
            ivm.Description = issue.Description;
            ivm.ProjectId = issue.ProjectId;
            ivm.ColumnId = issue.ColumnId.GetValueOrDefault();
            ivm.CreatedDate = issue.CreatedDate;
            ivm.UpdatedDate = issue.UpdatedDate;
            ivm.Columns = db.Columns.Where(c => c.Board.ProjectId == ivm.ProjectId).ToList();

            //ViewBag.ColumnId = new SelectList(db.Columns, "Id", "Name", issue.ColumnId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", issue.ProjectId);
            return View(ivm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,ProjectId,ColumnId,CreatedDate,UpdatedDate")] IssueViewModel ivm)
        {
            if (ModelState.IsValid)
            {
                //var projectId = db.Issues.Where(i => i.Id == ivm.Id).Select(i => i.ProjectId).FirstOrDefault();
                //if (ivm.ProjectId != projectId)
                //{
                //    ivm.ColumnId = null;
                //}
                Issue issue = new Issue();
                issue.Id = ivm.Id;
                issue.Name = ivm.Name;
                issue.Description = ivm.Description;
                issue.ProjectId = ivm.ProjectId;
                issue.ColumnId = ivm.ColumnId;
                issue.CreatedDate = ivm.CreatedDate;
                issue.UpdatedDate = DateTime.UtcNow;
                db.Entry(issue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Show", "BoardViewModel", new { id = issue.ProjectId });
            }
            //ViewBag.ColumnId = new SelectList(db.Columns, "Id", "Name", issue.ColumnId);
            //ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", issue.ProjectId);
            return View(ivm);
        }
    }
}