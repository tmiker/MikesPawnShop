using FluentValidation;
using Products.Write.Application.DTOs;

namespace Products.Write.Application.Validators
{
    public class AddImageDtoValidator : AbstractValidator<AddImageDTO>
    {
        public AddImageDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("A product Id must be provided.");
            RuleFor(x => x.Name)
                .NotEmpty().Length(0, 100).WithMessage($"A name must be provided.");
            RuleFor(x => x.Caption)
                .NotEmpty().Length(0, 100).WithMessage($"A caption must be provided.");
            RuleFor(x => x.ImageBlob)
                .NotNull().WithMessage($"An image blob must be provided.");
            RuleFor(x => x.BlobFileName)
                .NotEmpty().WithMessage($"A blob file name must be provided.");
        }
    }
}


//[Required]
//public string ProductId { get; init; } = default!;
//[Required]
//public string Name { get; init; } = default!;
//[Required]
//public string Caption { get; init; } = default!;

//// Added for blob service
//[Required]
//public IFormFile? ImageBlob { get; set; }
//[Required]
//public string? BlobFileName { get; set; }