using FluentValidation;
using NotBook.Api.Models.Comment.Request;

namespace NotBook.Api.Models.Comment.Validator
{
    public class CommentReportRequestValidator : AbstractValidator<CommentReportRequest>
    {
        public CommentReportRequestValidator()
        {
            RuleFor(request => request.CommentId).NotNull();
        }
    }
}