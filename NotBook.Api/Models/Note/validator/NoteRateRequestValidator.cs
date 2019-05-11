using FluentValidation;
using NotBook.Api.Models.Note.Request;

namespace NotBook.Api.Models.Note.validator
{
    public class NoteRateRequestValidator : AbstractValidator<NoteRateRequest>
    {
        public NoteRateRequestValidator()
        {
            RuleFor(request => request.NoteId).NotNull();
            RuleFor(request => request.Rate).NotNull();
        }
    }
}