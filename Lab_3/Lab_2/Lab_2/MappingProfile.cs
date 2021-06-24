using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Lab_2.Models;
using Lab_2.ViewModel;
using Lab_2.ViewModel.Order;

namespace Lab_2
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Expenses, ExpensesViewModel>();
            CreateMap<Comments, CommentsViewModel>();
            CreateMap<Expenses, ExpensesWithCommentsViewModel>();
            CreateMap<ExpensesInput, Expenses>();
            CreateMap<CommentsInput, Comments>();
            CreateMap<ApplicationUser, ApplicationUserViewModel>().ReverseMap();
            CreateMap<Orders, OrdersViewModel>();
            CreateMap<OrdersInput, Orders>();
        }


    }
}
