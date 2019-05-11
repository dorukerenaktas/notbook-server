using FluentValidation;
using NotBook.Api.Models.Note.Request;

namespace NotBook.Api.Models.Note.validator
{
    public class NoteReportRequestValidator : AbstractValidator<NoteReportRequest>
    {
        public NoteReportRequestValidator()
        {
            RuleFor(request => request.NoteId).NotNull();
        }
    }
}