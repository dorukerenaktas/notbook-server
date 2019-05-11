using Microsoft.AspNetCore.Http;

namespace NotBook.Api.Models.Note.Request
{
    public class NoteCreateRequest
    {
        public int LectureId { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public int Tag { get; set; }

        public IFormFile Document { get; set; }
    }
}