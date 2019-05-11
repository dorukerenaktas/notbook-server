using System.Linq;
using Dapper;
using NotBook.Core.Entity;
using NotBook.Data;
using NotBook.Data.MicroOrm;
using NotBook.Data.Repository;
using NotBook.Service.Post.DTOs;
using NotBook.Service.Post.Exception;
using NotBook.Service.User.DTOs;

namespace NotBook.Service.Post
{
    public class PostService : IPostService
    {
        private readonly IMicroOrmRepository _microOrmRepository;
        private readonly IRepository<PostEntity> _postRepository;
        private readonly IRepository<PostLikeEntity> _postLikeRepository;
        private readonly IRepository<PostReportEntity> _postReportRepository;

        public PostService(IMicroOrmRepository microOrmRepository,
            IRepository<PostEntity> postRepository,
            IRepository<PostLikeEntity> postLikeRepository,
            IRepository<PostReportEntity> postReportRepository)
        {
            _microOrmRepository = microOrmRepository;
            _postRepository = postRepository;
            _postLikeRepository = postLikeRepository;
            _postReportRepository = postReportRepository;
        }

        public int Create(int lectureId, string content, int createdBy, PostType type)
        {
            var post = new PostEntity
            {
                PostContent = content,
                PostParentIdFk = lectureId,
                PostCreatedByIdFk = createdBy,
                PostType = type,
                PostCreatedAt = System.DateTime.Now
            };
            
            _postRepository.Insert(post);
            _postRepository.SaveAll();

            return post.PostIdPk;
        }
        
        public PostDto[] ReadAll(int parentId, int userId, PostType type)
        {
            const string sql =
                "SELECT  PostIdPk, PostContent, PostIsEdited, PostCreatedAt," +
                "(SELECT  COUNT(*) FROM PostLike WHERE PostLikePostIdFk = PostIdPk) AS PostLikeCount," +
                "(SELECT  COUNT(*) FROM Comment WHERE CommentParentIdFk = PostIdPk AND CommentType = @commentType AND CommentIsDeleted != true) AS PostCommentCount," +
                "(SELECT  EXISTS(SELECT  * FROM PostLike WHERE PostLikeUserIdFk = @userId AND PostLikePostIdFk = PostIdPk)) AS PostIsLiked," +
                "UserIdPk, UserFirstName, UserLastName, UserImageName " +
                "FROM Post INNER JOIN User ON PostCreatedByIdFk = UserIdPk " +
                "WHERE PostType = @postType AND PostParentIdFk = @parentId AND PostIsDeleted != true " + 
                "ORDER BY PostCreatedAt DESC";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("parentId", parentId);
            dynamicParameters.Add("userId", userId);
            dynamicParameters.Add("postType", type);
            dynamicParameters.Add("commentType", CommentType.Post);
            var result = _microOrmRepository.Connection.Query<ExtendedPostEntity, UserEntity, PostDto>(sql,
                (post, user) => new PostDto
                {
                    Id = post.PostIdPk,
                    Content = post.PostContent,
                    IsEdited = post.PostIsEdited,
                    CreatedAt = post.PostCreatedAt,
                    LikeCount = post.PostLikeCount,
                    CommentCount = post.PostCommentCount,
                    IsLiked = post.PostIsLiked,
                    CreatedBy = new SimpleUserDto
                    {
                        Id = user.UserIdPk,
                        FirstName = user.UserFirstName,
                        LastName = user.UserLastName
                    }
                },
                dynamicParameters,
                splitOn: "UserIdPk").ToArray();
            
            return result;
        }
        
        public void Update(int postId, string content, int createdBy)
        {
            var post = _postRepository.Table.FirstOrDefault(x =>
                x.PostIdPk == postId && x.PostCreatedByIdFk == createdBy);
            
            if (post == null)
                throw new PostNotFoundException();

            post.PostContent = content;
            post.PostIsEdited = true;
            _postRepository.SaveAll();
        }
        
        public void Delete(int postId, int createdBy)
        {
            var post = _postRepository.Table.FirstOrDefault(x =>
                x.PostIdPk == postId && x.PostCreatedByIdFk == createdBy);
            
            if (post == null)
                throw new PostNotFoundException();

            post.PostIsDeleted = true;
            _postRepository.SaveAll();
        }
        
        public void Report(int postId, int userId)
        {
            if (_postReportRepository.Table.Any(x =>
                x.PostReportPostIdFk == postId && x.PostReportUserIdFk == userId))
                throw new PostAlreadyReportedException();
            
            var report = new PostReportEntity
            {
                PostReportPostIdFk = postId,
                PostReportUserIdFk = userId,
                PostReportCreatedAt = System.DateTime.Now
            };
           
            _postReportRepository.Insert(report);
            _postReportRepository.SaveAll();
        }

        public void Like(int postId, int userId)
        {
            if (_postLikeRepository.Table.Any(x =>
                x.PostLikePostIdFk == postId && x.PostLikeUserIdFk == userId))
                throw new PostAlreadyLikedException();

            var like = new PostLikeEntity
            {
                PostLikePostIdFk = postId,
                PostLikeUserIdFk = userId,
                PostLikeCreatedAt = System.DateTime.Now
            };
            
            _postLikeRepository.Insert(like);
            _postLikeRepository.SaveAll();
        }
        
        public void Unlike(int postId, int userId)
        {
            var like = _postLikeRepository.Table.FirstOrDefault(x =>
                x.PostLikePostIdFk == postId && x.PostLikeUserIdFk == userId);
            
            if (like == null)
                throw new PostNotLikedException();
            
            _postLikeRepository.Delete(like);
            _postLikeRepository.SaveAll();
        }
    }
}