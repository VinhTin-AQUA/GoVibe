using FluentValidation;
using GoVibe.API.Models.Places;

namespace GoVibe.API.Validators.Places
{
    public class PlaceValidators
    {
    }

    public class AddPlaceRequestValidator : AbstractValidator<AddPlaceRequest>
    {
        public AddPlaceRequestValidator()
        {
            var requiredMessage = "{PropertyName} is required";

            RuleFor(x => x.Name)
                .NotNull().WithMessage(requiredMessage)
                .NotEmpty().WithMessage(requiredMessage);

            RuleFor(x => x.Description)
                .NotNull().WithMessage(requiredMessage)
                .NotEmpty().WithMessage(requiredMessage);

            RuleFor(x => x.Address)
                .NotNull().WithMessage(requiredMessage)
                .NotEmpty().WithMessage(requiredMessage);

            RuleFor(x => x.Country)
                .NotNull().WithMessage(requiredMessage)
                .NotEmpty().WithMessage(requiredMessage);

            RuleFor(x => x.CategoryId)
                .NotNull().WithMessage(requiredMessage)
                .NotEmpty().WithMessage(requiredMessage);

            RuleFor(x => x.OpeningHours)
                .NotNull().WithMessage(requiredMessage)
                .NotEmpty().WithMessage(requiredMessage);
        }
    }

    public class UpdatePlaceRequestValidator : AbstractValidator<UpdatePlaceRequest>
    {
        public UpdatePlaceRequestValidator()
        {
            var requiredMessage = "{PropertyName} is required";

            RuleFor(x => x.Id)
                .NotNull().WithMessage(requiredMessage)
                .NotEmpty().WithMessage(requiredMessage);

            RuleFor(x => x.Name)
                .NotNull().WithMessage(requiredMessage)
                .NotEmpty().WithMessage(requiredMessage);

            RuleFor(x => x.Description)
                .NotNull().WithMessage(requiredMessage)
                .NotEmpty().WithMessage(requiredMessage);

            RuleFor(x => x.Address)
                .NotNull().WithMessage(requiredMessage)
                .NotEmpty().WithMessage(requiredMessage);

            RuleFor(x => x.Country)
                .NotNull().WithMessage(requiredMessage)
                .NotEmpty().WithMessage(requiredMessage);

            RuleFor(x => x.CategoryId)
                .NotNull().WithMessage(requiredMessage)
                .NotEmpty().WithMessage(requiredMessage);

            RuleFor(x => x.OpeningHours)
                .NotNull().WithMessage(requiredMessage)
                .NotEmpty().WithMessage(requiredMessage);
        }
    }
}
