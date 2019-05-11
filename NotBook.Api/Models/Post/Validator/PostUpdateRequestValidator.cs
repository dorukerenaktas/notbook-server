using FluentValidation;
using NotBook.Api.Models.Post.Request;

namespace NotBook.Api.Models.Post.Validator
{
    public class PostUpdateRequestValidator : AbstractValidator<PostUpdateRequest>
    {
        public PostUpdateRequestValidator()
        {
            RuleFor(request => request.PostId).NotNull();
            RuleFor(request => request.Content).NotEmpty();
        }
    }
}