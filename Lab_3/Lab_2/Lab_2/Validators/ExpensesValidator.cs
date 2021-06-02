using FluentValidation;
using Lab_2.Data;
using Lab_2.ViewModel;
using System;
using System.Linq;
using Type = Lab_2.Models.Type;

namespace Lab_2.Validators
{
    public class ExpensesValidator : AbstractValidator<ExpensesInput>
    {
        private readonly ApplicationDbContext _context;

        public ExpensesValidator(ApplicationDbContext context)
        {
            _context = context;
            RuleFor(x => x.Description).MinimumLength(10);
            RuleFor(x => x.Sum).InclusiveBetween(50, double.MaxValue);

            RuleFor(x => x.Type).Custom((prop, validationContext) =>
            {
                var instance = validationContext.InstanceToValidate;
                int commentsForTypeCount = _context.Comments.Where(e => e.Expenses.Type.Equals(instance.Type)).Count();
                if (commentsForTypeCount > 5)
                {
                    validationContext.AddFailure($"Cannot add an expense with type {instance.Type} " +
                        $"because that type has more then {commentsForTypeCount} comments");
                }
            });

            RuleFor(x => x.Date).LessThanOrEqualTo(DateTime.Now);

            //RuleFor(x=>x.Date).InclusiveBetween(2018-21-01,)

            //RuleFor(x => x.Type).InclusiveBetween(1, 8);
        }


    }
}
