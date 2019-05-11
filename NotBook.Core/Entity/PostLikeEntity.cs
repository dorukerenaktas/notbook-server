using System;
using System.ComponentModel.DataAnnotations;

namespace NotBook.Core.Entity
{
    public class PostLikeEntity
    {
        [Key] public int PostLikeIdPk { get; set; }
        
        public int PostLikePostIdFk { get; set; }
        
        public int PostLikeUserIdFk { get; set; }
        
        public DateTime PostLikeCreatedAt { get; set; }
    }
}