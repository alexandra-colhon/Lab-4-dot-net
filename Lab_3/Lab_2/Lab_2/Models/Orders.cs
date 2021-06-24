using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab_2.Models
{
    public class Orders
    {
        public int Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public IEnumerable<Expenses> Expenses { get; set; }
        public DateTime OrderDateTime { get; set; }

    }
}
