using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Lab_2.Data;
using Lab_2.Models;
using Lab_2.ViewModel.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Lab_2.Controllers
{
    [Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController:ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrdersController(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult> PlaceOrder(OrdersInput ordersInput)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            List<Expenses> orderedExpenses = new List<Expenses>();
            ordersInput.OrderExpensesIds.ForEach(eid =>
            {
                var expensesWithId = _context.Expenses.Find(eid);
                if (expensesWithId != null)
                {
                    orderedExpenses.Add(expensesWithId);
                }
            });

            if (orderedExpenses.Count == 0)
            {
                return BadRequest();
            }

            var order = new Orders
            {
                ApplicationUser = user,
                OrderDateTime = ordersInput.OrderDateTime.GetValueOrDefault(),
                Expenses = orderedExpenses
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return Ok();

        }


        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = _context.Orders.Where(o => o.ApplicationUser.Id == user.Id).Include(o => o.Expenses).FirstOrDefault();
            var resultViewModel = _mapper.Map<OrdersViewModel>(result);

            return Ok(resultViewModel);
        }

        
         [HttpGet("{id}")]
        public async Task<ActionResult<OrdersViewModel>> GetOrder(int id)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
         
            var order = _context.Orders.Where(o => o.ApplicationUser.Id == user.Id && o.Id == id).Include(o => o.Expenses).FirstOrDefault();
            if (order == null)
            {
                return NotFound();
            }
            var orderViewModel = _mapper.Map<OrdersViewModel>(order);

            return Ok(orderViewModel);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var order = _context.Orders.Where(o => o.ApplicationUser.Id == user.Id && o.Id == id).FirstOrDefault();
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrders(int id, OrdersInput ordersInput)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var order = _mapper.Map<Orders>(ordersInput);

            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool OrdersExists(int id)
        {
            var user = _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            //return _context.Orders.Where(o => o.ApplicationUser.Id == user.Id && o.Id == id).FirstOrDefault();
            return _context.Orders.Any(o => o.Id == id);
        }


    }
}
