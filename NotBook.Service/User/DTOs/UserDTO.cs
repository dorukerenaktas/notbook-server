using NotBook.Service.University.DTOs;

namespace NotBook.Service.User.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public int AttendedLectureCount { get; set; }
        
        public int AddedLectureCount { get; set; }
        
        public int FavNoteCount { get; set; }

        public int AddedNoteCount { get; set; }

        public SimpleUniversityDto University { get; set; }
    }
 
    public class SimpleUserDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}