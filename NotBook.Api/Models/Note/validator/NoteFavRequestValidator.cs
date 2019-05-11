using FluentValidation;
using NotBook.Api.Models.Note.Request;

namespace NotBook.Api.Models.Note.validator
{
    public class NoteFavRequestValidator : AbstractValidator<NoteFavRequest>
    {
        public NoteFavRequestValidator()
        {
            RuleFor(request => request.NoteId).NotNull();
        }
    }
}