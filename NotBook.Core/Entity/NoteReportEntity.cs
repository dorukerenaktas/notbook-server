using System;
using System.ComponentModel.DataAnnotations;

namespace NotBook.Core.Entity
{
    public class NoteReportEntity
    {
        [Key] public int NoteReportIdPk { get; set; }
        
        public int NoteReportNoteIdFk { get; set; }
        
        public int NoteReportUserIdFk { get; set; }
        
        public DateTime NoteReportCreatedAt { get; set; }
    }
}