using KanbanSDA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace KanbanSDA.DAL
{
    public class KanbanInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<KanbanContext>
    {
        public override void InitializeDatabase(KanbanContext context)
        {
            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, string.Format("ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE", context.Database.Connection.Database));

            base.InitializeDatabase(context);
        }

        protected override void Seed(KanbanContext context)
        {
            base.Seed(context);

            var projects = new List<Project>
            {
                new Project{Id=1, Name="Project 1", Description="First project"},
                new Project{Id=2, Name="Project 2", Description="Second project"}
            };
            projects.ForEach(p => context.Projects.Add(p));
            context.SaveChanges();

            var boards = new List<Board>
            {
                new Board{Id=1, ProjectId=1},
                new Board{Id=2, ProjectId=2}
            };
            boards.ForEach(b => context.Boards.Add(b));
            context.SaveChanges();

            var columns = new List<Column>
            {
                new Column{Id=1, Name="TO DO", BoardId=1},
                new Column{Id=2, Name="IN PROGRESS", BoardId=1},
                new Column{Id=3, Name="DONE", BoardId=1},
                new Column{Id=4, Name="TO DO", BoardId=2},
                new Column{Id=5, Name="IN PROGRESS", BoardId=2},
                new Column{Id=6, Name="DONE", BoardId=2}
            };
            columns.ForEach(c => context.Columns.Add(c));
            context.SaveChanges();

            var issues = new List<Issue>
            {
                new Issue{Id=1, Name="Zadanie 1", Description="Opis zadania 1", ProjectId=1, ColumnId=1, CreatedDate=DateTime.UtcNow, UpdatedDate=DateTime.UtcNow},
                new Issue{Id=2, Name="Zadanie 2", Description="Opis zadania 2", ProjectId=1, ColumnId=2, CreatedDate=DateTime.UtcNow, UpdatedDate=DateTime.UtcNow},
                new Issue{Id=3, Name="Zadanie 3", Description="Opis zadania 3", ProjectId=1, ColumnId=3, CreatedDate=DateTime.UtcNow, UpdatedDate=DateTime.UtcNow},
                new Issue{Id=4, Name="Zadanie 4", Description="Opis zadania 4", ProjectId=2, ColumnId=4, CreatedDate=DateTime.UtcNow, UpdatedDate=DateTime.UtcNow},
                new Issue{Id=5, Name="Zadanie 5", Description="Opis zadania 5", ProjectId=2, ColumnId=5, CreatedDate=DateTime.UtcNow, UpdatedDate=DateTime.UtcNow},
                new Issue{Id=6, Name="Zadanie 6", Description="Opis zadania 6", ProjectId=2, ColumnId=6, CreatedDate=DateTime.UtcNow, UpdatedDate=DateTime.UtcNow}
            };
            issues.ForEach(i => context.Issues.Add(i));
            context.SaveChanges();
        }
    }
}