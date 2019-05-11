using System;
using System.ComponentModel.DataAnnotations;

namespace NotBook.Core.Entity
{
    public class PostReportEntity
    {
        [Key] public int PostReportIdPk { get; set; }
        
        public int PostReportPostIdFk { get; set; }
        
        public int PostReportUserIdFk { get; set; }
        
        public DateTime PostReportCreatedAt { get; set; }
    }
}