using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KanbanSDA.Models
{
    public class Issue
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ProjectId { get; set; }
        public int? ColumnId { get; set; }
        //public int? Order { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public virtual Project Project { get; set; }
        public virtual Column Column { get; set; }
    }
}