using System.IO;
using FluentValidation;
using NotBook.Api.Models.User.Request;

namespace NotBook.Api.Models.User.Validator
{
    public class UserUpdateImageRequestValidator : AbstractValidator<UserUpdateImageRequest>
    {
        public UserUpdateImageRequestValidator()
        {
            RuleFor(request => request.Image).NotEmpty()
                .When(x => Path.GetExtension(x.Image.FileName) == "jpeg" ||
                           Path.GetExtension(x.Image.FileName) == "png");
        }
    }
}