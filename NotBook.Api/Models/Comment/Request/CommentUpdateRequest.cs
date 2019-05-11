namespace NotBook.Api.Models.Comment.Request
{
    public class CommentUpdateRequest
    {
        public int CommentId { get; set; }

        public string Content { get; set; }
    }
}