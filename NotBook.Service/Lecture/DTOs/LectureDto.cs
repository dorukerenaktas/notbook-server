using NotBook.Service.University.DTOs;

namespace NotBook.Service.Lecture.DTOs
{
    public class LectureDto
    {
        public int Id { get; set; }
        
        public string Code { get; set; }

        public string Name { get; set; }
        
        public int AttendCount { get; set; }
        
        public int PostCount { get; set; }
        
        public int NoteCount { get; set; }
        
        public bool IsAttended { get; set; }
        
        public SimpleUniversityDto  University { get; set; }
    }
    
    public class SimpleLectureDto
    {
        public int Id { get; set; }
        
        public string Code { get; set; }

        public string Name { get; set; }
        
        public SimpleUniversityDto  University { get; set; }
    }
}