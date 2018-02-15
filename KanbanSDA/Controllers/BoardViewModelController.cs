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
            bvm.IssuesList = db.Issues.Where(p => p.ProjectId == id).OrderBy(p => p.Position).ToList();

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
            issue.Position = 0;
            issue.UpdatedDate = DateTime.Now;
            db.Entry(issue).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Show", new { id = issue.ProjectId });
        }

        public ActionResult SendUp(int? issueId)
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

            if (issue.Position != 1)
            {
                Issue upperIssue = db.Issues.Where(p => p.Position == issue.Position - 1 && p.ColumnId == issue.ColumnId).FirstOrDefault();
                issue.Position = issue.Position - 1;
                upperIssue.Position = upperIssue.Position + 1;
                db.Entry(issue).State = EntityState.Modified;
                db.Entry(upperIssue).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Show", new { id = issue.ProjectId });
        }

        public ActionResult SendDown(int? issueId)
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

            Issue lowerIssue = db.Issues.Where(p => p.Position == issue.Position + 1 && p.ColumnId == issue.ColumnId).FirstOrDefault();
            if (lowerIssue != null)
            {
                issue.Position = issue.Position + 1;
                lowerIssue.Position = lowerIssue.Position - 1;
                db.Entry(issue).State = EntityState.Modified;
                db.Entry(lowerIssue).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Show", new { id = issue.ProjectId });
        }

        [HttpPost]
        public void SetNewPosition(int issueId, int columnId, int position)
        {
            Issue movedIssue = db.Issues.Find(issueId);
            var sourceColumnId = movedIssue.ColumnId.GetValueOrDefault();
            movedIssue.Position = position;
            movedIssue.ColumnId = columnId;
            db.Entry(movedIssue).State = EntityState.Modified;
            db.SaveChanges();

            BusinessLogic.BoardBL.ResetIssuesPosition(sourceColumnId);

            var issues = db.Issues.Where(c => c.ColumnId == columnId).OrderBy(p => p.Position).ToList();
            int pos = 1;
            foreach(Issue iss in issues)
            {
                if (pos == position && iss.Id != movedIssue.Id)
                {
                    iss.Position = pos + 1;
                    db.Entry(iss).State = EntityState.Modified;
                }
                else
                {
                    iss.Position = pos;
                    db.Entry(iss).State = EntityState.Modified;
                    pos++;
                }
            }
            db.SaveChanges();
        }
    }
}