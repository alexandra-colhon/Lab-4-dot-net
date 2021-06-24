using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab_2.ViewModel.Order
{
    public class OrdersInput
    {
        public List<int> OrderExpensesIds { get; set; }
        public DateTime? OrderDateTime { get; set; }
    }
}
