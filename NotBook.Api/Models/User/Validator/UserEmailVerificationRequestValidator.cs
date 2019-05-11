using FluentValidation;
using NotBook.Api.Models.User.Request;

namespace NotBook.Api.Models.User.Validator
{
    public class UserEmailVerificationRequestValidator : AbstractValidator<UserEmailVerificationRequest>
    {
        public UserEmailVerificationRequestValidator()
        {
            RuleFor(request => request.Hash).NotEmpty();
        }
    }
}