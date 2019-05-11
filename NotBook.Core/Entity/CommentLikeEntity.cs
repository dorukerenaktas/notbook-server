using System;
using System.ComponentModel.DataAnnotations;

namespace NotBook.Core.Entity
{
    public class CommentLikeEntity
    {
        [Key] public int CommentLikeIdPk { get; set; }
        
        public int CommentLikeCommentIdFk { get; set; }
        
        public int CommentLikeUserIdFk { get; set; }
        
        public DateTime CommentLikeCreatedAt { get; set; }
    }
}