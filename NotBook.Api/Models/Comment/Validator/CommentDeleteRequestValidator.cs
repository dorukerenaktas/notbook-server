using FluentValidation;
using NotBook.Api.Models.Comment.Request;

namespace NotBook.Api.Models.Comment.Validator
{
    public class CommentDeleteRequestValidator : AbstractValidator<CommentDeleteRequest>
    {
        public CommentDeleteRequestValidator()
        {
            RuleFor(request => request.CommentId).NotNull();
        }
    }
}