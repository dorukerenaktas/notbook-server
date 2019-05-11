using FluentValidation;
using NotBook.Api.Models.Note.Request;

namespace NotBook.Api.Models.Note.validator
{
    public class NoteUpdateRequestValidator : AbstractValidator<NoteUpdateRequest>
    {
        public NoteUpdateRequestValidator()
        {
            RuleFor(request => request.NoteId).NotNull();
            RuleFor(request => request.Name).NotEmpty();
            RuleFor(request => request.Description).NotEmpty();
            RuleFor(request => request.Tag).NotNull();
        }
    }
}