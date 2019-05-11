using NotBook.Service.University.DTOs;

namespace NotBook.Service.University
{
    public interface IUniversityService
    {
        UniversityDto Read(int universityId);
    }
}