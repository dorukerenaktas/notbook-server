namespace NotBook.Api.Models.Comment.Request
{
    public class CommentCreateRequest
    {
        public int ParentId { get; set; }

        public string Content { get; set; }

        public int Type { get; set; }
    }
}