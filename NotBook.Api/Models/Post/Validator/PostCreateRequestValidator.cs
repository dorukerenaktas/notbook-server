using FluentValidation;
using NotBook.Api.Models.Post.Request;

namespace NotBook.Api.Models.Post.Validator
{
    public class PostCreateRequestValidator : AbstractValidator<PostCreateRequest>
    {
        public PostCreateRequestValidator()
        {
            RuleFor(request => request.ParentId).NotNull();
            RuleFor(request => request.Content).NotEmpty();
            RuleFor(request => request.Type).NotNull();
        }
    }
}