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
            Board board = db.Boards.Find(id);
            if (board == null)
            {
                return HttpNotFound();
            }
            BoardViewModel bvm = new BoardViewModel();
            bvm.Board = board;
            bvm.ColumnsList = GetColumnsListWithBoardId(id.GetValueOrDefault());
            bvm.IssuesList = GetAllIssues(); // Do zmiany na GetIssuesWithBoardId

            return View(bvm);
        }

        private List<Column> GetColumnsListWithBoardId(int id)
        {
            var columns = db.Columns.Where(c => c.BoardId == id).ToList();
            return columns;
        }

        // Do zmiany na GetIssuesWithBoardId
        private List<Issue> GetAllIssues()
        {
            var issues = db.Issues.Select(i => i).ToList();
            return issues;
        }

        //private List<Issue> GetIssuesWithBoardId(int id)
        //{
            
        //}
    }
}