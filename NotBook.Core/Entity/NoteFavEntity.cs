using System;
using System.ComponentModel.DataAnnotations;

namespace NotBook.Core.Entity
{
    public class NoteFavEntity
    {
        [Key] public int NoteFavIdPk { get; set; }
        
        public int NoteFavNoteIdFk { get; set; }
        
        public int NoteFavUserIdFk { get; set; }
        
        public DateTime NoteFavCreatedAt { get; set; }
    }
}