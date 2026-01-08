using FluentValidation;
using Products.Write.Application.DTOs;

namespace Products.Write.Application.Validators
{
    public class AddProductDtoValidator : AbstractValidator<AddProductDTO>
    {
        public AddProductDtoValidator()
        {
            // NotEmpty checks NotNull also

            var validCategories = new List<string> { "Female", "Male" };

            RuleFor(x => x.Name)
                .NotEmpty().Length(0, 100).WithMessage("A name must be provided.");
            RuleFor(x => x.Category)
                .NotEmpty().Length(0, 50).WithMessage($"A category must be provided.");
            RuleFor(x => x.Description)
                .NotEmpty().Length(0, 255).WithMessage($"A description must be provided.");
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage($"A price greater than 0 must be provided.");
            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage($"A currency must be provided.");
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage($"A status must be provided.");
            RuleFor(x => x.QuantityOnHand)
                .NotEmpty().GreaterThanOrEqualTo(0).WithMessage($"A quantity on hand greater than or equal to 0 must be provided.");
            RuleFor(x => x.UOM)
                .NotEmpty().WithMessage($"Unit of measure must be provided.");
            RuleFor(x => x.LowStockThreshold)
                .NotEmpty().GreaterThanOrEqualTo(0).WithMessage($"A low stock threshold greater than or equal to 0 must be provided.");

            // Async
            //RuleFor(x => x.Id).MustAsync(async (id, cancellation) =>
            //    {
            //        bool exists = await _client.IdExists(id);
            //        return !exists;
            //    }).WithMessage("ID Must be unique");
        }
    }
}


//[Required]
//public string Name { get; init; } = default!;
//[Required]
//public string Category { get; init; }
//[Required]
//public string Description { get; init; } = default!;
//[Required]
//public decimal Price { get; init; }
//[Required]
//public string Currency { get; init; } = default!;
//[Required]
//public string Status { get; init; } = default!;
//[Required]
//public int QuantityOnHand { get; init; }
//[Required]
//public string UOM { get; init; } = default!;
//[Required]
//public int LowStockThreshold { get; init; }