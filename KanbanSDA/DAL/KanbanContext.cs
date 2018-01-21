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

            modelBuilder.Entity<Board>()
                       .HasMany(e => e.Columns)
                       .WithRequired(e => e.Board)
                       .HasForeignKey<int>(e => e.BoardId)
                       .WillCascadeOnDelete(false);

            modelBuilder.Entity<Column>()
                        .HasMany(e => e.Issues)
                        .WithOptional(e => e.Column)
                        .HasForeignKey<int?>(e => e.ColumnId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<Project>()
                        .HasOptional(e => e.Board)
                        .WithRequired(e => e.Project)
                        .WillCascadeOnDelete(true);

            modelBuilder.Entity<Project>()
                        .HasMany(e => e.Issues)
                        .WithRequired(e => e.Project)
                        .HasForeignKey<int>(e => e.ProjectId)
                        .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Project>()
            //            .HasOptional(b => b.Board)
            //            .WithRequired(p => p.Project)
            //            .WillCascadeOnDelete(true);

            //modelBuilder.Entity<Project>()
            //            .HasMany(i => i.Issues)
            //            .WithOptional(p => p.Project)
            //            .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Column>()
            //            .HasRequired<Board>(b => b.Board)
            //            .WithMany(c => c.Columns)
            //            .HasForeignKey<int>(b => b.BoardId);

            //modelBuilder.Entity<Issue>()
            //            .HasOptional<Column>(c => c.Column)
            //            .WithMany(i => i.Issues)
            //            .HasForeignKey<int>(c => c.ColumnId);
        }

        public System.Data.Entity.DbSet<KanbanSDA.ViewModels.IssueViewModel> IssueViewModels { get; set; }
    }
}