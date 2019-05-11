using System;
using System.Linq;
using Dapper;
using NotBook.Core.Entity;
using NotBook.Data.MicroOrm;
using NotBook.Data.Repository;
using NotBook.Service.Comment.DTOs;
using NotBook.Service.Comment.Exception;
using NotBook.Service.User.DTOs;

namespace NotBook.Service.Comment
{
    public class CommentService : ICommentService
    {
        private readonly IMicroOrmRepository _microOrmRepository;
        private readonly IRepository<CommentEntity> _commentRepository;
        private readonly IRepository<CommentLikeEntity> _commentLikeRepository;
        private readonly IRepository<CommentReportEntity> _commentReportRepository;

        public CommentService(IMicroOrmRepository microOrmRepository,
            IRepository<CommentEntity> commentRepository,
            IRepository<CommentLikeEntity> commentLikeRepository,
            IRepository<CommentReportEntity> commentReportRepository)
        {
            _microOrmRepository = microOrmRepository;
            _commentRepository = commentRepository;
            _commentLikeRepository = commentLikeRepository;
            _commentReportRepository = commentReportRepository;
        }
        
        public int Create(int parentId, string content, int createdBy, CommentType type)
        {
            var comment = new CommentEntity
            {
                CommentContent = content,
                CommentParentIdFk = parentId,
                CommentCreatedByIdFk = createdBy,
                CommentType = type,
                CommentCreatedAt = DateTime.Now
            };
            
            _commentRepository.Insert(comment);
            _commentRepository.SaveAll();

            return comment.CommentIdPk;
        }
        
        public CommentDto[] ReadAll(int parentId, CommentType type, int userId)
        {
            const string sql =
                "SELECT  CommentIdPk, CommentContent, CommentIsEdited, CommentCreatedAt," +
                "(SELECT  COUNT(*) FROM CommentLike WHERE CommentLikeCommentIdFk = CommentIdPk) AS CommentLikeCount," +
                "(SELECT  EXISTS(SELECT  * FROM CommentLike WHERE CommentLikeUserIdFk = @userId AND CommentLikeCommentIdFk = CommentIdPk)) AS CommentIsLiked," +
                "UserIdPk, UserFirstName, UserLastName " +
                "FROM Comment INNER JOIN User ON CommentCreatedByIdFk = UserIdPk " +
                "WHERE CommentParentIdFk = @parentId AND CommentType = @commentType AND CommentIsDeleted != true " + 
                "ORDER BY CommentCreatedAt ASC";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("parentId", parentId);
            dynamicParameters.Add("userId", userId);
            dynamicParameters.Add("commentType", type);
            var result = _microOrmRepository.Connection.Query<ExtendedCommentEntity, UserEntity, CommentDto>(sql,
                (comment, user) => new CommentDto
                {
                    Id = comment.CommentIdPk,
                    Content = comment.CommentContent,
                    IsEdited = comment.CommentIsEdited,
                    CreatedAt = comment.CommentCreatedAt,
                    LikeCount = comment.CommentLikeCount,
                    IsLiked = comment.CommentIsLiked,
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
        
        public void Update(int commentId, string content, int createdBy)
        {
            var comment = _commentRepository.Table.FirstOrDefault(x =>
                x.CommentIdPk == commentId && x.CommentCreatedByIdFk == createdBy);
            
            if (comment == null)
                throw new CommentNotFoundException();

            comment.CommentContent = content;
            comment.CommentIsEdited = true;
            _commentRepository.SaveAll();
        }
        
        public void Delete(int commentId, int createdBy)
        {
            var comment = _commentRepository.Table.FirstOrDefault(x =>
                x.CommentIdPk == commentId && x.CommentCreatedByIdFk == createdBy);
            
            if (comment == null)
                throw new CommentNotFoundException();

            comment.CommentIsDeleted = true;
            _commentRepository.SaveAll();
        }
        
        public void Report(int commentId, int userId)
        {
            if (_commentReportRepository.Table.Any(x =>
                x.CommentReportCommentIdFk == commentId && x.CommentReportUserIdFk == userId))
                throw new CommentAlreadyReportedException();
            
            var report = new CommentReportEntity
            {
                CommentReportCommentIdFk = commentId,
                CommentReportUserIdFk = userId,
                CommentReportCreatedAt = DateTime.Now
            };
           
            _commentReportRepository.Insert(report);
            _commentReportRepository.SaveAll();
        }
        
        public void Like(int commentId, int userId)
        {
            if (_commentLikeRepository.Table.Any(x =>
                x.CommentLikeCommentIdFk == commentId && x.CommentLikeUserIdFk == userId))
                throw new CommentAlreadyLikedException();

            var like = new CommentLikeEntity
            {
                CommentLikeCommentIdFk = commentId,
                CommentLikeUserIdFk = userId,
                CommentLikeCreatedAt = DateTime.Now
            };
            
            _commentLikeRepository.Insert(like);
            _commentLikeRepository.SaveAll();
        }
        
        public void Unlike(int commentId, int userId)
        {
            var like = _commentLikeRepository.Table.FirstOrDefault(x =>
                x.CommentLikeCommentIdFk == commentId && x.CommentLikeUserIdFk == userId);
            
            if (like == null)
                throw new CommentNotLikedException();
            
            _commentLikeRepository.Delete(like);
            _commentLikeRepository.SaveAll();
        }
    }
}