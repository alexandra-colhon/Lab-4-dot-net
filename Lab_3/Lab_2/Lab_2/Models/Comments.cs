using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab_2.Models
{
    public class Comments
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool Important { get; set; }
        public Expenses Expenses { get; set; }
    }
}
