using NotBook.Service.Lecture.DTOs;
using NotBook.Service.Post.DTOs;

namespace NotBook.Service.Lecture
{
    public interface ILectureService
    {
        int Create(string code, string name, int universityId, int createdBy);
        
        LectureDto Read(int lectureId, int userId);
        
        SimpleLectureDto[] ReadAll(int universityId);
        
        SimpleLectureDto[] ReadAttended(int userId);
        
        SimpleLectureDto[] ReadAdded(int userId);

        void Attend(int lectureId, int userId);

        void Quit(int lectureId, int userId);
    }
}