using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KanbanSDA.Models
{
    public class Column
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BoardId { get; set; }

        public Board Board { get; set; }
        public List<Issue> Issues { get; set; }
    }
}