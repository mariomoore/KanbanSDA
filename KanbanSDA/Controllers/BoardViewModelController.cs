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
    public class BoardViewModelController : Controller
    {
        private KanbanContext db = new KanbanContext();

        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BoardViewModel bvm = new BoardViewModel();
            bvm.BoardsList = db.Boards.Where(b => b.ProjectId == id).ToList();
            bvm.ColumnsList = db.Columns.Where(c => c.Board.ProjectId == id).ToList();
            bvm.IssuesList = db.Issues.Where(p => p.ProjectId == id).ToList();

            return View(bvm);
        }

        public ActionResult SendToBacklog(int? issueId)
        {
            if (issueId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Issue issue = db.Issues.Find(issueId);
            if (issue == null)
            {
                return HttpNotFound();
            }
            issue.ColumnId = null;
            issue.UpdatedDate = DateTime.UtcNow;
            db.Entry(issue).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Show", new { id = issue.ProjectId });
        }

    }
}