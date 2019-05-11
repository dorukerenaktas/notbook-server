using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotBook.Api.Models.Comment.Request;
using NotBook.Core.Constants;
using NotBook.Core.Entity;
using NotBook.Core.Models;
using NotBook.Service.Authentication;
using NotBook.Service.Comment;
using NotBook.Service.Comment.Exception;

namespace NotBook.Api.Controllers
{
    [Authorize]
    [Route(Constants.Version + "/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IAuthenticationService _authenticationService;

        public CommentController(ICommentService commentService, IAuthenticationService authenticationService)
        {
            _commentService = commentService;
            _authenticationService = authenticationService;
        }
        
        [HttpPost]
        public IActionResult Create([FromBody] CommentCreateRequest request)
        {
            try
            {
                var userId = _authenticationService.GetAuthenticatedUserId(User);
                var commentType = (CommentType) Enum.ToObject(typeof(CommentType), request.Type);
                var commentId = _commentService.Create(request.ParentId, request.Content, userId, commentType);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success,
                    CommentId = commentId
                });
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }
        
        [HttpGet("all")]
        public IActionResult ReadAll([FromQuery] int parentId, int type)
        {
            try
            {
                var userId = _authenticationService.GetAuthenticatedUserId(User);
                var commentType = (CommentType) Enum.ToObject(typeof(CommentType), type);
                var comments = _commentService.ReadAll(parentId, commentType, userId);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success,
                    Comments = comments
                });
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }
        
        [HttpPut]
        public IActionResult Update([FromBody] CommentUpdateRequest request)
        {
            try
            {
                var userId = _authenticationService.GetAuthenticatedUserId(User);
                _commentService.Update(request.CommentId, request.Content, userId);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success,
                });
            }
            catch (CommentNotFoundException)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }
        
        [HttpDelete]
        public IActionResult Delete([FromBody] CommentDeleteRequest request)
        {
            try
            {
                var userId = _authenticationService.GetAuthenticatedUserId(User);
                _commentService.Delete(request.CommentId, userId);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success,
                });
            }
            catch (CommentNotFoundException)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }
        
        [HttpPost("report")]
        public IActionResult Report([FromBody] CommentReportRequest request)
        {
            try
            {
                var userId = _authenticationService.GetAuthenticatedUserId(User);
                _commentService.Report(request.CommentId, userId);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success,
                });
            }
            catch (CommentAlreadyReportedException)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.CommentAlreadyReported});
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }
        
        [HttpPost("like")]
        public IActionResult Like([FromBody] CommentLikeRequest request)
        {
            try
            {
                var userId = _authenticationService.GetAuthenticatedUserId(User);
                _commentService.Like(request.CommentId, userId);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success,
                });
            }
            catch (CommentAlreadyLikedException)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }
        
        [HttpDelete("like")]
        public IActionResult Unlike([FromBody] CommentUnlikeRequest request)
        {
            try
            {
                var userId = _authenticationService.GetAuthenticatedUserId(User);
                _commentService.Unlike(request.CommentId, userId);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success,
                });
            }
            catch (CommentNotLikedException)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }
    }
}