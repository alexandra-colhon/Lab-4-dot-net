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
using Type = Lab_2.Models.Type;
using Microsoft.AspNetCore.Authorization;

namespace Lab_2.Controllers
{
    //[Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
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

        /// <summary>
        /// Filters the expenses betweend certain dates and with a specific type
        /// </summary>
        /// <param name="from">the first date</param>
        /// <param name="to">until date</param>
        /// <param name="type">type of expenses</param>
        /// <returns>A list of expenses with the date bigger from date and less then to date and with the given type</returns>
        // GET: api/Expenses/filter
        [HttpGet("{DateTime & DateTime & Type}/filter")]
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


        /// <summary>
        /// Filters the expenses betweend certain dates and with a specific type
        /// </summary>
        /// <param name="from">the first date</param>
        /// <param name="to">until date</param>
        /// <param name="type">type of expenses</param>
        /// <returns>A list of expenses with the date bigger from date and less then to date and with the given type</returns>
        // GET: api/Expenses/filterlambda
        [HttpGet("{DateTime & DateTime & Type}/lambdafilter")]
        public ActionResult<IEnumerable<ExpensesViewModel>> FilterLambdaExpenses(DateTime from, DateTime to, string type)
        {
            var query = _context.Expenses.Where(e => e.Date >= from && e.Date <= to && e.Type == type).Select(e => _mapper.Map<ExpensesViewModel>(e));
            return query.ToList();

        }


        /// <summary>
        /// Returns the comments for a given expense
        /// </summary>
        /// <param name="id">the id of the expense</param>
        /// <returns>A list with all the comments of a certain expense</returns>
        // GET: api/Expenses/5/Comments
        [HttpGet("{id}/comments")]
        public ActionResult<IEnumerable<ExpensesWithCommentsViewModel>> GetCommentsForExpenses(int id)
        {
            var query = _context.Expenses.Where(e => e.Id == id).Include(e => e.Comments).Select(e => _mapper.Map<ExpensesWithCommentsViewModel>(e));
            return query.ToList();
        }


        /// <summary>
        /// A comment for an expense is added
        /// </summary>
        /// <param name="id">the id of the expense</param>
        /// <param name="commentInput">the added comment </param>
        /// <returns>Returns 1 if the comment was added and an error if not</returns>
        // POST: api/Expenses/5/Comments
        [HttpPost("{id}/comments")]
        public IActionResult PostCommentForExpenses(int id, CommentsInput commentInput)
        {
            var comment = _mapper.Map<Comments>(commentInput);

            comment.Expenses = _context.Expenses.Find(id);
            if (comment.Expenses == null)
            {
                return NotFound();
            }
            _context.Comments.Add(comment);
            _context.SaveChanges();

            return Ok();
        }


        /// <summary>
        /// Returns all the existing expenses 
        /// </summary>
        /// <returns>A list with all the expenses</returns>
        // GET: api/Expenses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpensesViewModel>>> GetExpenses()
        {
            var query = _context.Expenses.Select(e => _mapper.Map<ExpensesViewModel>(e));
            return await query.ToListAsync();
        }


        /// <summary>
        /// Returns an expense with a given id
        /// </summary>
        /// <param name="id">the id of the expense</param>
        /// <returns>Returns the expense if the id is valid</returns>
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


        /// <summary>
        /// Updates an expense if it exists
        /// </summary>
        /// <param name="id">the id of the expense to update</param>
        /// <param name="expensesInput">the expense to update</param>
        /// <returns>Returns 1 if the update did take place</returns>
        // PUT: api/Expenses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpenses(int id, ExpensesInput expensesInput)
        {
            var expenses = _mapper.Map<Expenses>(expensesInput);

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


        /// <summary>
        /// A new expense is added
        /// </summary>
        /// <param name="expensesInput">the expense to be added</param>
        /// <returns>Returns the expense</returns>
        // POST: api/Expenses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Expenses>> PostExpenses(ExpensesInput expensesInput)
        {
            var expenses = _mapper.Map<Expenses>(expensesInput);

            _context.Expenses.Add(expenses);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExpenses", new { id = expenses.Id }, expenses);
        }


        /// <summary>
        /// A certain expense is deleted
        /// </summary>
        /// <param name="id">the id of the expese to delete</param>
        /// <returns>Returns 1 if the expense is deleted</returns>
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
