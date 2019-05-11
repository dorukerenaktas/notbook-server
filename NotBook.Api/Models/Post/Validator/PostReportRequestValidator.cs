using FluentValidation;
using NotBook.Api.Models.Post.Request;

namespace NotBook.Api.Models.Post.Validator
{
    public class PostReportRequestValidator : AbstractValidator<PostReportRequest>
    {
        public PostReportRequestValidator()
        {
            RuleFor(request => request.PostId).NotNull();
        }
    }
}