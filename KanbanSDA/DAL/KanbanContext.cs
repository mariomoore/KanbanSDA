using KanbanSDA.Models;
using KanbanSDA.ViewModels;
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
        public DbSet<IssueViewModel> IssueViewModels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Project>()
                        .HasOptional(e => e.Board)
                        .WithRequired(e => e.Project)
                        .WillCascadeOnDelete(true);

            modelBuilder.Entity<Project>()
                        .HasMany(e => e.Issues)
                        .WithOptional(p => p.Project)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<Board>()
                        .HasMany(e => e.Columns)
                        .WithRequired(e => e.Board)
                        .HasForeignKey<int>(e => e.BoardId)
                        .WillCascadeOnDelete(true);

            modelBuilder.Entity<Column>()
                        .HasMany(e => e.Issues)
                        .WithOptional(e => e.Column)
                        .HasForeignKey<int?>(e => e.ColumnId)
                        .WillCascadeOnDelete(false);
        }
    }
}