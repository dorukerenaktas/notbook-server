using FluentValidation;
using NotBook.Api.Models.Comment.Request;

namespace NotBook.Api.Models.Comment.Validator
{
    public class CommentLikeRequestValidator : AbstractValidator<CommentLikeRequest>
    {
        public CommentLikeRequestValidator()
        {
            RuleFor(request => request.CommentId).NotNull();
        }
    }
}