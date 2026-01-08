using FluentValidation;
using Products.Write.Application.DTOs;

namespace Products.Write.Application.Validators
{
    public class PurgeDataDtoValidator : AbstractValidator<PurgeDataDTO>
    {
        public PurgeDataDtoValidator()
        {
            RuleFor(x => x.PinNumber)
                .NotEmpty().WithMessage("A pin number must be provided.");
        }
    }
}


//[Required]
//[DisplayName("Pin Number")]
//public int PinNumber { get; init; }