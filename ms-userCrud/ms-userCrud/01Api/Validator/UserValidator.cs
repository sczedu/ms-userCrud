using FluentValidation;
using ms_userCrud._01Api.Model;
using System;

namespace ms_userCrud._01Api.Validator
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(c => c)
                .NotNull()
                .OnAnyFailure(x =>
                {
                    throw new ArgumentNullException("User can not be null");
                });
            RuleFor(c => c.Username)
                .NotEmpty().WithMessage("Username is required")
                .NotNull().WithMessage("Username is required");
            RuleFor(c => c.Document)
                .NotEmpty().WithMessage("Document is required")
                .NotNull().WithMessage("Document is required");
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Name is required")
                .NotNull().WithMessage("Name is required");
            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("Email is required")
                .NotNull().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is required");
            RuleFor(c => c.Password)
                .NotEmpty().WithMessage("Password is required")
                .NotNull().WithMessage("Password is required");
        }
    }
}

