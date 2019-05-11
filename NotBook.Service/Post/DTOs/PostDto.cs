using System;
using NotBook.Service.User.DTOs;

namespace NotBook.Service.Post.DTOs
{
    public class PostDto
    {
        public int Id { get; set; }
        
        public string Content { get; set; }
        
        public int LikeCount { get; set; }
        
        public int CommentCount { get; set; }
        
        public bool IsLiked { get; set; }
        
        public bool IsEdited { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public SimpleUserDto CreatedBy { get; set; }
    }
}