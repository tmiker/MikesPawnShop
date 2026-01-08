using FluentValidation;
using Products.Write.Application.DTOs;

namespace Products.Write.Application.Validators
{
    public class AddDocumentDtoValidator : AbstractValidator<AddDocumentDTO>
    {
        public AddDocumentDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("A product Id must be provided.");
            RuleFor(x => x.Name)
                .NotEmpty().Length(0, 100).WithMessage($"A name must be provided.");
            RuleFor(x => x.Title)
                .NotEmpty().Length(0, 100).WithMessage($"A title must be provided.");
            RuleFor(x => x.DocumentBlob)
                .NotNull().WithMessage($"An document blob must be provided.");
            RuleFor(x => x.BlobFileName)
                .NotEmpty().WithMessage($"A blob file name must be provided.");
        }
    }
}

//public class AddDocumentDTO
//{
//    [Required]
//    public string ProductId { get; init; } = default!;
//    [Required]
//    public string Name { get; init; } = default!;
//    [Required]
//    public string Title { get; init; } = default!;

//    // Added for blob service
//    [Required]
//    public IFormFile? DocumentBlob { get; set; }
//    [Required]
//    public string? BlobFileName { get; set; }
//}