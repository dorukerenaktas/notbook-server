using NotBook.Core.Entity;
using NotBook.Service.Comment.DTOs;

namespace NotBook.Service.Comment
{
    public interface ICommentService
    {
        int Create(int postId, string content, int createdBy, CommentType type);
        
        CommentDto[] ReadAll(int parentId, CommentType type, int userId);

        void Update(int commentId, string content, int createdBy);

        void Delete(int commentId, int createdBy);

        void Report(int commentId, int userId);

        void Like(int commentId, int userId);

        void Unlike(int commentId, int userId);
    }
}