using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab_2.ViewModel.Authentication
{
    public class ConfirmUserInput
    {
        public string Email { get; set; }
        public string ConfirmationToken { get; set; }
    }
}
