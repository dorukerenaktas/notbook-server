using System;
using System.ComponentModel.DataAnnotations;

namespace NotBook.Core.Entity
{
    public class NoteRateEntity
    {
        [Key] public int NoteRateIdPk { get; set; }
        
        public int NoteRateNoteIdFk { get; set; }
        
        public int NoteRateUserIdFk { get; set; }
        
        public float NoteRateValue { get; set; }
        
        public DateTime NoteRateCreatedAt { get; set; }
    }
}