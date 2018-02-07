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
    public class ColumnController : Controller
    {
        private KanbanContext db = new KanbanContext();

        // GET: Column
        public ActionResult Index()
        {
            var columns = db.Columns.Include(c => c.Board);
            return View(columns.ToList());
        }

        // GET: Column/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Column column = db.Columns.Find(id);
            if (column == null)
            {
                return HttpNotFound();
            }
            return View(column);
        }

        // GET: Column/Create
        public ActionResult Create(int? boardId)
        {
            if (boardId.HasValue)
            {
                Column column = new Column();
                column.BoardId = boardId.GetValueOrDefault();
                
                return View(column);
            }
            else
            {
                return View();
            }
        }

        // POST: Column/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,BoardId")] Column column)
        {
            if (ModelState.IsValid)
            {
                db.Columns.Add(column);
                db.SaveChanges();
                return RedirectToAction("Show", "BoardViewModel", new { id = column.BoardId });
            }

            return View(column);
        }

        // GET: Column/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Column column = db.Columns.Find(id);
            if (column == null)
            {
                return HttpNotFound();
            }
            
            return View(column);
        }

        // POST: Column/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,BoardId")] Column column)
        {
            if (ModelState.IsValid)
            {
                db.Entry(column).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Show", "BoardViewModel", new { id = column.BoardId });
            }
            
            return View(column);
        }

        // GET: Column/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Column column = db.Columns.Find(id);
            if (column == null)
            {
                return HttpNotFound();
            }
            return View(column);
        }

        // POST: Column/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var issues = db.Issues.Where(c => c.ColumnId == id);
            foreach (Issue iss in issues)
            {
                iss.ColumnId = null;
            }
            db.SaveChanges();
            Column column = db.Columns.Find(id);
            db.Columns.Remove(column);
            db.SaveChanges();
            return RedirectToAction("Show", "BoardViewModel", new { id = column.BoardId });
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
