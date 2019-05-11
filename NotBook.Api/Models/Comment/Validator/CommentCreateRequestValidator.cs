using FluentValidation;
using NotBook.Api.Models.Comment.Request;

namespace NotBook.Api.Models.Comment.Validator
{
    public class CommentCreateRequestValidator : AbstractValidator<CommentCreateRequest>
    {
        public CommentCreateRequestValidator()
        {
            RuleFor(request => request.ParentId).NotNull();
            RuleFor(request => request.Content).NotEmpty();
            RuleFor(request => request.Type).NotNull();
        }
    }
}