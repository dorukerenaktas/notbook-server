using FluentValidation;
using NotBook.Api.Models.Post.Request;

namespace NotBook.Api.Models.Post.Validator
{
    public class PostUnlikeRequestValidator : AbstractValidator<PostUnlikeRequest>
    {
        public PostUnlikeRequestValidator()
        {
            RuleFor(request => request.PostId).NotNull();
        }
    }
}