using FluentValidation;
using NotBook.Api.Models.Lecture.Request;

namespace NotBook.Api.Models.Lecture.Validator
{
    public class LectureAttendRequestValidator : AbstractValidator<LectureAttendRequest>
    {
        public LectureAttendRequestValidator()
        {
            RuleFor(request => request.LectureId).NotNull();
        }
    }
}