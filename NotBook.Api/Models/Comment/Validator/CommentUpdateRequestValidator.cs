using FluentValidation;
using NotBook.Api.Models.Comment.Request;

namespace NotBook.Api.Models.Comment.Validator
{
    public class CommentUpdateRequestValidator : AbstractValidator<CommentUpdateRequest>
    {
        public CommentUpdateRequestValidator()
        {
            RuleFor(request => request.CommentId).NotNull();
            RuleFor(request => request.Content).NotEmpty();
        }
    }
}