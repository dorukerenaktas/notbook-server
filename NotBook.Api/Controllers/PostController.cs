using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotBook.Api.Models.Post.Request;
using NotBook.Core.Constants;
using NotBook.Core.Entity;
using NotBook.Core.Models;
using NotBook.Service.Authentication;
using NotBook.Service.Post;
using NotBook.Service.Post.Exception;

namespace NotBook.Api.Controllers
{
    [Authorize]
    [Route(Constants.Version + "/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IAuthenticationService _authenticationService;

        public PostController(IPostService postService, IAuthenticationService authenticationService)
        {
            _postService = postService;
            _authenticationService = authenticationService;
        }
        
        [HttpPost]
        public IActionResult Create([FromBody] PostCreateRequest request)
        {
            try
            {
                var userId = _authenticationService.GetAuthenticatedUserId(User);
                var postType = (PostType) Enum.ToObject(typeof(PostType), request.Type);
                var postId = _postService.Create(request.ParentId, request.Content, userId, postType);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success,
                    PostId = postId
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
                var postType = (PostType) Enum.ToObject(typeof(PostType), type);
                var posts = _postService.ReadAll(parentId, userId, postType);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success,
                    Posts = posts
                });
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }
        
        [HttpPut]
        public IActionResult Update([FromBody] PostUpdateRequest request)
        {
            try
            {
                var userId = _authenticationService.GetAuthenticatedUserId(User);
                _postService.Update(request.PostId, request.Content, userId);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success,
                });
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }
        
        [HttpDelete]
        public IActionResult Delete([FromBody] PostDeleteRequest request)
        {
            try
            {
                var userId = _authenticationService.GetAuthenticatedUserId(User);
                _postService.Delete(request.PostId, userId);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success,
                });
            }
            catch (PostNotFoundException)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }
        
        [HttpPost("report")]
        public IActionResult Report([FromBody] PostReportRequest request)
        {
            try
            {
                var userId = _authenticationService.GetAuthenticatedUserId(User);
                _postService.Report(request.PostId, userId);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success,
                });
            }
            catch (PostAlreadyReportedException)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.PostAlreadyReported});
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }
        
        [HttpPost("like")]
        public IActionResult Like([FromBody] PostLikeRequest request)
        {
            try
            {
                var userId = _authenticationService.GetAuthenticatedUserId(User);
                _postService.Like(request.PostId, userId);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success,
                });
            }
            catch (PostAlreadyLikedException)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }
        
        [HttpDelete("like")]
        public IActionResult Unlike([FromBody] PostUnlikeRequest request)
        {
            try
            {
                var userId = _authenticationService.GetAuthenticatedUserId(User);
                _postService.Unlike(request.PostId, userId);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success,
                });
            }
            catch (PostNotLikedException)
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