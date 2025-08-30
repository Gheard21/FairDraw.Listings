using System;
using FairDraw.Listings.App.Application.Models.Requests;
using FluentValidation;

namespace FairDraw.Listings.App.Application.Validators;

public class NewListingRequestValidator : AbstractValidator<NewListingRequest>
{
    public NewListingRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");
    }
}
