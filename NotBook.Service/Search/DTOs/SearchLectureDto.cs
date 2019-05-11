using NotBook.Service.University.DTOs;

namespace NotBook.Service.Search.DTOs
{
    public class SearchLectureDto
    {
        public int Id { get; set; }
        
        public string Code { get; set; }
        
        public string Name { get; set; }
        
        public SimpleUniversityDto University { get; set; }
    }
}