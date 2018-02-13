using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KanbanSDA.Models
{
    public class Issue
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [DisplayName("Project")]
        public int? ProjectId { get; set; }
        [DisplayName("Column")]
        public int? ColumnId { get; set; }
        public int Position { get; set; }
        [DisplayName("Created date")]
        public DateTime CreatedDate { get; set; }
        [DisplayName("Updated date")]
        public DateTime UpdatedDate { get; set; }

        public virtual Project Project { get; set; }
        public virtual Column Column { get; set; }
    }
}