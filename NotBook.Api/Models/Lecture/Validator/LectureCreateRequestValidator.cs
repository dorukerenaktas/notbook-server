using FluentValidation;
using NotBook.Api.Models.Lecture.Request;

namespace NotBook.Api.Models.Lecture.Validator
{
    public class LectureCreateRequestValidator : AbstractValidator<LectureCreateRequest>
    {
        public LectureCreateRequestValidator()
        {
            RuleFor(request => request.Code).NotEmpty();
            RuleFor(request => request.Name).NotEmpty();
        }
    }
}