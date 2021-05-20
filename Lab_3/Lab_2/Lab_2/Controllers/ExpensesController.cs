using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab_2.Data;
using Lab_2.Models;
using Lab_2.ViewModel;
using AutoMapper;

namespace Lab_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ExpensesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // GET: api/Expenses/filter
        [HttpGet("{DateTime & DateTime & string}/filter")]
        public ActionResult<IEnumerable<Expenses>> FilterExpenses(DateTime from, DateTime to, string type)
        {
            var expenses = _context.Expenses.ToList();
            List<Expenses> filtered = new List<Expenses>();

            foreach (var expense in expenses)
            {
                if (expense.Date >= from && expense.Date <= to && expense.Type == type)
                {
                    filtered.Add(expense);
                }
            }
            return filtered.ToList();
    
        }


        // GET: api/Expenses/filterlambda
        [HttpGet("{DateTime & DateTime & string}/filterlambda")]
        public ActionResult<IEnumerable<Expenses>> FilterLambdaExpenses(DateTime from, DateTime to, string type)
        {
            var query = _context.Expenses.Where(e => e.Date >= from && e.Date<=to && e.Type == type);
            return query.ToList();

        }


        // GET: api/Expenses/5/Comments
        [HttpGet("{id}/comments")]
        public ActionResult<IEnumerable<ExpensesWithCommentsViewModel>> GetCommentsForExpenses(int id)
        {
            var query = _context.Expenses.Where(e => e.Id == id).Include(e => e.Comments).Select(e => _mapper.Map<ExpensesWithCommentsViewModel>(e));
            return query.ToList();
        }


        // POST: api/Expenses/5/Comments
        [HttpPost("{id}/comments")]
        public IActionResult PostCommentForExpenses(int id, Comments comment)
        {
            comment.Expenses = _context.Expenses.Find(id);
            if(comment.Expenses == null)
            {
                return NotFound();
            }
            _context.Comments.Add(comment);
            _context.SaveChanges();

            return Ok();
        }


        // GET: api/Expenses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expenses>>> GetExpenses()
        {
            return await _context.Expenses.ToListAsync();
        }


        // GET: api/Expenses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExpensesViewModel>> GetExpenses(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }
            var expenseViewModel = _mapper.Map<ExpensesViewModel>(expense);

            return expenseViewModel;
        }


        // PUT: api/Expenses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpenses(int id, Expenses expenses)
        {
            if (id != expenses.Id)
            {
                return BadRequest();
            }

            _context.Entry(expenses).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpensesExists(id))
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


        // POST: api/Expenses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Expenses>> PostExpenses(Expenses expenses)
        {
            _context.Expenses.Add(expenses);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExpenses", new { id = expenses.Id }, expenses);
        }


        // DELETE: api/Expenses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpenses(int id)
        {
            var expenses = await _context.Expenses.FindAsync(id);
            if (expenses == null)
            {
                return NotFound();
            }

            _context.Expenses.Remove(expenses);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool ExpensesExists(int id)
        {
            return _context.Expenses.Any(e => e.Id == id);
        }
    }
}
