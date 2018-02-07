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
        
        // Czy potrzebne?
        //public System.Data.Entity.DbSet<KanbanSDA.ViewModels.IssueViewModel> IssueViewModels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Project>()
                        .HasMany(b => b.Boards)
                        .WithRequired(p => p.Project)
                        .HasForeignKey<int>(p => p.ProjectId)
                        .WillCascadeOnDelete(true);

            modelBuilder.Entity<Project>()
                        .HasMany(i => i.Issues)
                        .WithOptional(p => p.Project)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<Board>()
                        .HasMany(c => c.Columns)
                        .WithRequired(b => b.Board)
                        .HasForeignKey<int>(b => b.BoardId)
                        .WillCascadeOnDelete(true);

            modelBuilder.Entity<Column>()
                        .HasMany(i => i.Issues)
                        .WithOptional(c => c.Column)
                        .HasForeignKey<int?>(c => c.ColumnId)
                        .WillCascadeOnDelete(false);
        }
    }
}