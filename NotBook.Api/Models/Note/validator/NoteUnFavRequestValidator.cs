using FluentValidation;
using NotBook.Api.Models.Note.Request;

namespace NotBook.Api.Models.Note.validator
{
    public class NoteUnFavRequestValidator : AbstractValidator<NoteUnFavRequest>
    {
        public NoteUnFavRequestValidator()
        {
            RuleFor(request => request.NoteId).NotNull();
        }
    }
}