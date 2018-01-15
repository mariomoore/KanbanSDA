using KanbanSDA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KanbanSDA.DAL
{
    public class KanbanInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<KanbanContext>
    {
        protected override void Seed(KanbanContext context)
        {
            base.Seed(context);

            var projects = new List<Project>
            {
                new Project{Id=1, Name="Project 1", Description="First project"}
            };
            projects.ForEach(p => context.Projects.Add(p));
            context.SaveChanges();

            var boards = new List<Board>
            {
                new Board{Id=1}
            };
            boards.ForEach(b => context.Boards.Add(b));
            context.SaveChanges();

            var columns = new List<Column>
            {
                new Column{Id=1, Name="TO DO", BoardId=1},
                new Column{Id=2, Name="IN PROGRESS", BoardId=1},
                new Column{Id=3, Name="DONE", BoardId=1}
            };
            columns.ForEach(c => context.Columns.Add(c));
            context.SaveChanges();

            var issues = new List<Issue>
            {
                new Issue{Id=1, Name="Zadanie 1", Description="Opis zadania 1", ColumnId=1, CreatedDate=DateTime.UtcNow, UpdatedDate=DateTime.UtcNow},
                new Issue{Id=2, Name="Zadanie 2", Description="Opis zadania 2", ColumnId=2, CreatedDate=DateTime.UtcNow, UpdatedDate=DateTime.UtcNow}
            };
            issues.ForEach(i => context.Issues.Add(i));
            context.SaveChanges();
        }
    }
}