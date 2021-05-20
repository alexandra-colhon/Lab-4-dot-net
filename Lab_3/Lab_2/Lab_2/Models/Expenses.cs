using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab_2.Models
{
    public class Expenses
    {

        public int Id { get; set; }

        [MinLength(10)]
        public string Description { get; set; }

        [Required]
        [Range(50, Double.MaxValue)]
        public double Sum { get; set; }

        public string Location { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public string Type { get; set; }

        public List<Comments> Comments { get; set; }

    }
}
