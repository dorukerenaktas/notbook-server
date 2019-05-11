namespace NotBook.Api.Models.Note.Request
{
    public class NoteUpdateRequest
    {
        public int NoteId { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public int Tag { get; set; }
    }
}