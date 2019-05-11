namespace NotBook.Api.Models.Post.Request
{
    public class PostCreateRequest
    {
        public int ParentId { get; set; }

        public string Content { get; set; }

        public int Type { get; set; }
    }
}