using KanbanSDA.DAL;
using KanbanSDA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace KanbanSDA.BusinessLogic
{
    public static class BoardBL
    {
        public static void ResetIssuesPosition(int columnId)
        {
            KanbanContext db = new KanbanContext();
            var issues = db.Issues.Where(c => c.ColumnId == columnId).OrderBy(p => p.Position).ToList();
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