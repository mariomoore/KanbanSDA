using KanbanSDA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace KanbanSDA.DAL
{
    public class KanbanContext : DbContext
    {
        public KanbanContext() : base()
        {

        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<Issue> Issues { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Project>()
                        .HasOptional(b => b.Board)
                        .WithRequired(p => p.Project);

            modelBuilder.Entity<Column>()
                        .HasRequired<Board>(b => b.Board)
                        .WithMany(c => c.Columns)
                        .HasForeignKey<int>(b => b.BoardId);

            modelBuilder.Entity<Issue>()
                        .HasRequired<Column>(c => c.Column)
                        .WithMany(i => i.Issues)
                        .HasForeignKey<int>(c => c.ColumnId);
        }
    }
}