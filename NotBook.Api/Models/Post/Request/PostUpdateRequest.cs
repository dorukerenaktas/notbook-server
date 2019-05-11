namespace NotBook.Api.Models.Post.Request
{
    public class PostUpdateRequest
    {
        public int PostId { get; set; }

        public string Content { get; set; }
    }
}