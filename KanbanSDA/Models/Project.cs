﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KanbanSDA.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int BoardId { get; set; }

        public virtual Board Board { get; set; }
    }
}