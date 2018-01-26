using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KanbanSDA.Models
{
    public class Board
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }

        public virtual List<Column> Columns { get; set; }
        public virtual Project Project { get; set; }
    }
}