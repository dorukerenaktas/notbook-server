using FluentValidation;
using NotBook.Api.Models.Lecture.Request;

namespace NotBook.Api.Models.Lecture.Validator
{
    public class LectureQuitRequestValidator : AbstractValidator<LectureQuitRequest>
    {
        public LectureQuitRequestValidator()
        {
            RuleFor(request => request.LectureId).NotNull();
        }
    }
}