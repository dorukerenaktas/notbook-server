using FluentValidation;
using NotBook.Api.Models.Note.Request;

namespace NotBook.Api.Models.Note.validator
{
    public class NoteCreateRequestValidator : AbstractValidator<NoteCreateRequest>
    {
        public NoteCreateRequestValidator()
        {
            RuleFor(request => request.LectureId).NotNull();
            RuleFor(request => request.Name).NotEmpty();
            RuleFor(request => request.Description).NotEmpty();
            RuleFor(request => request.Tag).NotNull();
            RuleFor(request => request.Document).NotEmpty();
        }
    }
}