using FluentValidation;
using NotBook.Api.Models.User.Request;

namespace NotBook.Api.Models.User.Validator
{
    public class UserRefreshTokenRequestRequestValidator : AbstractValidator<UserRefreshTokenRequest>
    {
        public UserRefreshTokenRequestRequestValidator()
        {
            RuleFor(request => request.AccessToken).NotEmpty();
            RuleFor(request => request.RefreshToken).NotEmpty();
        }
    }
}