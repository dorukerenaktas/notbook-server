namespace NotBook.Service.University.DTOs
{
    public class UniversityDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Abbr { get; set; }

        public string ImageUrl { get; set; }
        
        public int StudentCount { get; set; }
        
        public int LectureCount { get; set; }
        
        public int CommentCount { get; set; }
        
        public int Location { get; set; }
    }
    
    public class SimpleUniversityDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Abbr { get; set; }

        public string ImageUrl { get; set; }
    }
    
}