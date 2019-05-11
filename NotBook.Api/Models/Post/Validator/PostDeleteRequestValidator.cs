using FluentValidation;
using NotBook.Api.Models.Post.Request;

namespace NotBook.Api.Models.Post.Validator
{
    public class PostDeleteRequestValidator : AbstractValidator<PostDeleteRequest>
    {
        public PostDeleteRequestValidator()
        {
            RuleFor(request => request.PostId).NotNull();
        }
    }
}