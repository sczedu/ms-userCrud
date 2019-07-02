using FluentValidation;
using ms_userCrud._01Api;
using ms_userCrud._01Api.Model;
using System;

namespace ms_userCrud._01Api.Validator
{
    public class UserAuthValidator : AbstractValidator<User>
    {

        public UserAuthValidator()
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
            RuleFor(c => c.Password)
                .NotEmpty().WithMessage("Password is required")
                .NotNull().WithMessage("Password is required");
        }
    }
}

