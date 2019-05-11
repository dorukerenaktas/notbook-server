using Microsoft.AspNetCore.Http;
using NotBook.Core.Entity;
using NotBook.Service.Common.DTOs;
using NotBook.Service.Note.DTOs;

namespace NotBook.Service.Note
{
    public interface INoteService
    {
        int Create(int lectureId, string name, string description, NoteTag tag, IFormFile document, int createdBy);
        
        NoteDto[] ReadAll(int lectureId, int userId);
        
        NoteDto[] ReadFav(int userId);
        
        NoteDto[] ReadAdded(int userId);
        
        FileDto ReadDocument(int noteId);
        
        void Update(int noteId, string name, string description, NoteTag tag, int userId);

        void Report(int noteId, int userId);
        
        void Fav(int noteId, int userId);

        void UnFav(int noteId, int userId);

        float Rate(int noteId, float value, int userId);
    }
}