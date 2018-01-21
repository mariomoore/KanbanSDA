using KanbanSDA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KanbanSDA.ViewModels
{
    public class IssueViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProjectId { get; set; }
        public int ColumnId { get; set; }
        public IEnumerable<Column> Columns { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        //public List<Column> Columns { get; set; }
    }
}