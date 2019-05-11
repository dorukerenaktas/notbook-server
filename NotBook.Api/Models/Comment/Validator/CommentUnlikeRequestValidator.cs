using FluentValidation;
using NotBook.Api.Models.Comment.Request;

namespace NotBook.Api.Models.Comment.Validator
{
    public class CommentUnlikeRequestValidator : AbstractValidator<CommentUnlikeRequest>
    {
        public CommentUnlikeRequestValidator()
        {
            RuleFor(request => request.CommentId).NotNull();
        }
    }
}