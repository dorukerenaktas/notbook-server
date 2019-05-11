using System;
using NotBook.Service.Lecture.DTOs;
using NotBook.Service.User.DTOs;

namespace NotBook.Service.Note.DTOs
{
    public class NoteDto
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public float Rate { get; set; }
        
        public int Tag { get; set; }
        
        public int FavCount { get; set; }
        
        public int CommentCount { get; set; }
                
        public bool IsFav { get; set; }
        
        public bool IsEdited { get; set; }
        
        public string FileExtension { get; set; }
                
        public SimpleLectureDto Lecture { get; set; }
        
        public SimpleUserDto CreatedBy { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }
}