using System;
using System.ComponentModel.DataAnnotations;

namespace NotBook.Core.Entity
{
    public class CommentReportEntity
    {
        [Key] public int CommentReportIdPk { get; set; }
        
        public int CommentReportCommentIdFk { get; set; }
        
        public int CommentReportUserIdFk { get; set; }
        
        public DateTime CommentReportCreatedAt { get; set; }
    }
}