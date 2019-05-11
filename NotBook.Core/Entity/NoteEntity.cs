using System;
using System.ComponentModel.DataAnnotations;

namespace NotBook.Core.Entity
{
    public class NoteEntity
    {
        [Key] public int NoteIdPk { get; set; }
        
        public int NoteLectureIdFk { get; set; }
        
        public int NoteCreatedByIdFk { get; set; }
        
        public string NoteName { get; set; }
        public string NoteDescription { get; set; }
        
        public NoteTag NoteTag { get; set; }
        
        public string NoteFileName { get; set; }
                
        public bool NoteIsEdited { get; set; }
        
        public DateTime NoteCreatedAt { get; set; }
    }
    
    public class ExtendedNoteEntity
    {
        [Key] public int NoteIdPk { get; set; }
        
        public int NoteLectureIdFk { get; set; }
        
        public int NoteCreatedByIdFk { get; set; }
        
        public string NoteName { get; set; }
        public string NoteDescription { get; set; }
        
        public float NoteRate { get; set; }
        
        public int NoteFavCount { get; set; }
        
        public int NoteCommentCount { get; set; }
        
        public bool NoteIsFav { get; set; }
        
        public bool NoteIsEdited { get; set; }
               
        public NoteTag NoteTag { get; set; }
        
        public string NoteFileName { get; set; }
        
        public DateTime NoteCreatedAt { get; set; }
    }
    
    public enum NoteTag
    {
        Notebook = 0,
        Slide = 1,
        Lab = 2,
        Homework = 3,
        Project = 4,
        Quiz = 5,
        Midterm = 6,
        Final =7
    }
}