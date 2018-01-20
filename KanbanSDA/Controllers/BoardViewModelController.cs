using KanbanSDA.DAL;
using KanbanSDA.Models;
using KanbanSDA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace KanbanSDA.Controllers
{
    public class BoardViewModelController : Controller
    {
        private KanbanContext db = new KanbanContext();

        // GET: BoardViewModel
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Project project = db.Projects.Find(id);
            //if (project == null)
            //{
            //    return HttpNotFound();
            //}

            BoardViewModel bvm = new BoardViewModel();
            //bvm.Project = project;
            bvm.Board = db.Boards.Where(b => b.ProjectId == id).FirstOrDefault();
            bvm.ColumnsList = GetColumnsListWithBoardId(bvm.Board.Id);
            bvm.IssuesList = GetIssuesWithProjectId(id.GetValueOrDefault()); // Do zmiany na GetIssuesWithBoardId

            return View(bvm);
        }

        private List<Column> GetColumnsListWithBoardId(int id)
        {
            var columns = db.Columns.Where(c => c.BoardId == id).ToList();
            return columns;
        }

        // Do zmiany na GetIssuesWithBoardId
        //private List<Issue> GetAllIssues()
        //{
        //    var issues = db.Issues.Select(i => i).ToList();
        //    return issues;
        //}

        private List<Issue> GetIssuesWithProjectId(int id)
        {
            var issues = db.Issues.Where(p => p.ProjectId == id).ToList();
            return issues;
        }
    }
}