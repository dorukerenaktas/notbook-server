using NotBook.Service.Search.DTOs;

namespace NotBook.Service.Search
{
    public interface ISearchService
    {
        SearchUniversityDto[] SearchUniversity(string query);
        
        SearchLectureDto[] SearchLecture(string query);
    }
}