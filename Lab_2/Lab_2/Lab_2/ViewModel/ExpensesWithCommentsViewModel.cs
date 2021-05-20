using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab_2.ViewModel
{
    public class ExpensesWithCommentsViewModel
    {

        public int Id { get; set; }
        public string Description { get; set; }
        public double Sum { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public string Currency { get; set; }
        public string Type { get; set; } 
        public IEnumerable<CommentsViewModel> Comments { get; set; }

    }
}
