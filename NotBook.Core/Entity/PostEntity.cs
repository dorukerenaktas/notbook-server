using System;
using System.ComponentModel.DataAnnotations;

namespace NotBook.Core.Entity
{
    public class PostEntity
    {
        [Key] public int PostIdPk { get; set; }
        
        public int PostCreatedByIdFk { get; set; }
        
        public int PostParentIdFk { get; set; }
        
        public string PostContent { get; set; }
        
        public PostType PostType { get; set; }
        
        public bool PostIsEdited { get; set; }
        
        public bool PostIsDeleted { get; set; }

        public DateTime PostCreatedAt { get; set; }
    }
    
    public class ExtendedPostEntity
    {
        [Key] public int PostIdPk { get; set; }
        
        public int PostCreatedByIdFk { get; set; }
        
        public int PostParentIdFk { get; set; }
        
        public string PostContent { get; set; }
        
        public PostType PostType { get; set; }
        
        public int PostLikeCount { get; set; }
        
        public int PostCommentCount { get; set; }
        
        public bool PostIsLiked { get; set; }
        
        public bool PostIsEdited { get; set; }
        
        public bool PostIsDeleted { get; set; }

        public DateTime PostCreatedAt { get; set; }
    }
    
    public enum PostType
    {
        Announcement = 0,
        Suggestion = 1,
        University = 2
    }
}