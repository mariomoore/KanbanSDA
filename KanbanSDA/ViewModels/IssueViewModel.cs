using KanbanSDA.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DisplayName("Project")]
        public Nullable<int> ProjectId { get; set; }
        public string ProjectName { get; set; }
        public List<SelectListItem> ProjectsList;
        [DisplayName("Column")]
        public Nullable<int> ColumnId { get; set; }
        public List<SelectListItem> ColumnsList;
        //public int Position { get; set; }
        [DisplayName("Created date")]
        public DateTime CreatedDate { get; set; }
        [DisplayName("Updated date")]
        public DateTime UpdatedDate { get; set; }

        public virtual Project Project { get; set; }
        public virtual Column Column { get; set; }
    }
}