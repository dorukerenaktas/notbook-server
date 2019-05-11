using FluentValidation;
using NotBook.Api.Models.User.Request;

namespace NotBook.Api.Models.User.Validator
{
    public class UserResendVerificationRequestValidator : AbstractValidator<UserResendVerificationRequest>
    {
        public UserResendVerificationRequestValidator()
        {
            RuleFor(request => request.Email).NotEmpty().EmailAddress();
        }
    }
}