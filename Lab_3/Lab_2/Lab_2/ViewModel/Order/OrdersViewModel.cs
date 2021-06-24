using Lab_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab_2.ViewModel.Order
{
    public class OrdersViewModel
    {
        public ApplicationUserViewModel ApplicationUser { get; set; }
        public List<ExpensesViewModel> Expenses { get; set; }
        public DateTime OrderDateTime { get; set; }
    }
}
