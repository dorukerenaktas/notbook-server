using FluentValidation;
using NotBook.Api.Models.User.Request;

namespace NotBook.Api.Models.User.Validator
{
    public class UserPasswordVerificationRequestValidator : AbstractValidator<UserPasswordVerificationRequest>
    {
        public UserPasswordVerificationRequestValidator()
        {
            RuleFor(request => request.Hash).NotEmpty();
            RuleFor(request => request.Password).NotEmpty();
        }
    }
}