using FluentValidation;
using NotBook.Api.Models.Post.Request;

namespace NotBook.Api.Models.Post.Validator
{
    public class PostLikeRequestValidator : AbstractValidator<PostLikeRequest>
    {
        public PostLikeRequestValidator()
        {
            RuleFor(request => request.PostId).NotNull();
        }
    }
}