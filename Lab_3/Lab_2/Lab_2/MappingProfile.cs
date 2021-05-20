using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Lab_2.Models;
using Lab_2.ViewModel;

namespace Lab_2
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Expenses, ExpensesViewModel>();
            CreateMap<Comments, CommentsViewModel>();
            CreateMap<Expenses, ExpensesWithCommentsViewModel>();
        }


    }
}
