using System;
using System.ComponentModel.DataAnnotations;

namespace NotBook.Core.Entity
{
    public class CommentEntity
    {
        [Key] public int CommentIdPk { get; set; }
        
        public int CommentParentIdFk { get; set; }
        
        public int CommentCreatedByIdFk { get; set; }
        
        public string CommentContent { get; set; }
        
        public CommentType CommentType { get; set; }
        
        public bool CommentIsEdited { get; set; }
        
        public bool CommentIsDeleted { get; set; }

        public DateTime CommentCreatedAt { get; set; }
    }
    
    public class ExtendedCommentEntity
    {
        [Key] public int CommentIdPk { get; set; }
        
        public int CommentParentIdFk { get; set; }
        
        public int CommentCreatedByIdFk { get; set; }
        
        public string CommentContent { get; set; }
        
        public CommentType CommentType { get; set; }
        
        public int CommentLikeCount { get; set; }
        
        public bool CommentIsLiked { get; set; }
        
        public bool CommentIsEdited { get; set; }
        
        public bool CommentIsDeleted { get; set; }

        public DateTime CommentCreatedAt { get; set; }
    }
    
    public enum CommentType
    {
        Post = 0,
        Note = 1
    }
}