using NotBook.Core.Entity;
using NotBook.Service.Post.DTOs;

namespace NotBook.Service.Post
{
    public interface IPostService
    {
        int Create(int lectureId, string content, int createdBy, PostType type);
        
        PostDto[] ReadAll(int parentId, int userId, PostType type);

        void Update(int postId, string content, int createdBy);

        void Delete(int postId, int createdBy);

        void Report(int postId, int userId);

        void Like(int postId, int userId);

        void Unlike(int postId, int userId);
    }
}