using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotBook.Api.Models.Lecture.Request;
using NotBook.Core.Constants;
using NotBook.Core.Models;
using NotBook.Service.Authentication;
using NotBook.Service.Lecture;
using NotBook.Service.Lecture.Exception;
using NotBook.Service.User.Exception;

namespace NotBook.Api.Controllers
{
    [Authorize]
    [Route(Constants.Version + "/[controller]")]
    [ApiController]
    public class LectureController : ControllerBase
    {
        private readonly ILectureService _lectureService;
        private readonly IAuthenticationService _authenticationService;

        public LectureController(ILectureService lectureService, IAuthenticationService authenticationService)
        {
            _lectureService = lectureService;
            _authenticationService = authenticationService;
        }
        
        [HttpPost]
        public IActionResult Create([FromBody] LectureCreateRequest request)
        {
            try
            {
                var userId = _authenticationService.GetAuthenticatedUserId(User);
                var universityId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "UniversityId")?.Value);
                var lectureId = _lectureService.Create(request.Code, request.Name, universityId, userId);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success,
                    LectureId = lectureId
                });
            }
            catch (LectureAlreadyExistsException)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.LectureAlreadyExist});
            }
            catch (UserNotFoundException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }
        
        [HttpGet]
        public IActionResult Read([FromQuery] int lectureId)
        {
            try
            {
                var userId = _authenticationService.GetAuthenticatedUserId(User);
                var lecture = _lectureService.Read(lectureId, userId);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success,
                    Data = lecture
                });
            }
            catch (LectureNotFoundException)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.LectureNotFound});
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }
        
        [HttpGet("all")]
        public IActionResult ReadAll([FromQuery] int universityId)
        {
            try
            {
                var lectures= _lectureService.ReadAll(universityId);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success,
                    Data = lectures
                });
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }
        
        [HttpGet("attended")]
        public IActionResult ReadAttended()
        {
            try
            {
                var userId = _authenticationService.GetAuthenticatedUserId(User);
                var lectures= _lectureService.ReadAttended(userId);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success,
                    Data = lectures
                });
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }
        
        [HttpGet("added")]
        public IActionResult ReadAdded()
        {
            try
            {
                var userId = _authenticationService.GetAuthenticatedUserId(User);
                var lectures= _lectureService.ReadAdded(userId);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success,
                    Data = lectures
                });
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }
        
        [HttpPost("attend")]
        public IActionResult Attend([FromBody] LectureAttendRequest request)
        {
            try
            {
                var userId = _authenticationService.GetAuthenticatedUserId(User);
                _lectureService.Attend(request.LectureId, userId);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success
                });
            }
            catch (UserAlreadyAttendedException)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }
        
        [HttpDelete("attend")]
        public IActionResult Quit([FromBody] LectureQuitRequest request)
        {
            try
            {
                var userId = _authenticationService.GetAuthenticatedUserId(User);
                _lectureService.Quit(request.LectureId, userId);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success
                });
            }
            catch (UserNotAttendedException)
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