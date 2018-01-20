using KanbanSDA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KanbanSDA.ViewModels
{
    public class BoardViewModel
    {
        //public Project Project { get; set; }
        public Board Board { get; set; }
        public List<Column> ColumnsList { get; set; }
        public List<Issue> IssuesList { get; set; }
    }
}