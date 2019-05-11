using FluentValidation;
using NotBook.Api.Models.User.Request;

namespace NotBook.Api.Models.User.Validator
{
    public class UserLoginRequestValidator : AbstractValidator<UserLoginRequest>
    {
        public UserLoginRequestValidator()
        {
            RuleFor(request => request.Email).NotEmpty().EmailAddress();
            RuleFor(request => request.Password).NotNull().NotEmpty();
        }
    }
}