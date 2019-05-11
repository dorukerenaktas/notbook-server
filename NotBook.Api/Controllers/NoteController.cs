using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotBook.Api.Models.Note.Request;
using NotBook.Core.Constants;
using NotBook.Core.Entity;
using NotBook.Core.Models;
using NotBook.Service.Authentication;
using NotBook.Service.Note;
using NotBook.Service.Note.Exception;

namespace NotBook.Api.Controllers
{
    [Authorize]
    [Route(Constants.Version + "/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;
        private readonly IAuthenticationService _authenticationService;

        public NoteController(INoteService noteService, IAuthenticationService authenticationService)
        {
            _noteService = noteService;
            _authenticationService = authenticationService;
        }
        
        [HttpPost]
        public IActionResult Create([FromForm] NoteCreateRequest request)
        {
            try
            {
                var userId = _authenticationService.GetAuthenticatedUserId(User);
                var tag = (NoteTag) Enum.ToObject(typeof(NoteTag), request.Tag);
                
                var noteId = _noteService.Create(request.LectureId, request.Name, request.Description, tag, request.Document, userId);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success,
                    NoteId = noteId
                });
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }
        
        [HttpGet("all")]
        public IActionResult ReadAll([FromQuery] int lectureId)
        {
            try
            {
                var userId = _authenticationService.GetAuthenticatedUserId(User);
                var notes = _noteService.ReadAll(lectureId, userId);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success,
                    Notes = notes
                });
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }
        
        [HttpGet("fav")]
        public IActionResult ReadFav()
        {
            try
            {
                var userId = _authenticationService.GetAuthenticatedUserId(User);
                var notes = _noteService.ReadFav(userId);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success,
                    Data = notes
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
                var notes = _noteService.ReadAdded(userId);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success,
                    Data = notes
                });
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }
        
        [AllowAnonymous]
        [HttpGet("document")]
        public IActionResult ReadDocument([FromQuery] int noteId)
        {
            try
            {
                var noteFile = _noteService.ReadDocument(noteId);
                return File(noteFile.FileStream, "application/octet-stream", noteFile.FileName + noteFile.FileExtension);
            }
            catch (NoteNotFoundException)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }
        
        [HttpPut]
        public IActionResult Update([FromBody] NoteUpdateRequest request)
        {
            try
            {
                var userId = _authenticationService.GetAuthenticatedUserId(User);
                var tag = (NoteTag) Enum.ToObject(typeof(NoteTag), request.Tag);
                _noteService.Update(request.NoteId, request.Name, request.Description, tag, userId);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success,
                });
            }
            catch (NoteNotFoundException)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }
        
        [HttpPost("report")]
        public IActionResult Report([FromBody] NoteReportRequest request)
        {
            try
            {
                var userId = _authenticationService.GetAuthenticatedUserId(User);
                _noteService.Report(request.NoteId, userId);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success
                });
            }
            catch (NoteAlreadyReportedException)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.NoteAlreadyReported});
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }
        
        [HttpPost("fav")]
        public IActionResult Fav([FromBody] NoteFavRequest request)
        {
            try
            {
                var userId = _authenticationService.GetAuthenticatedUserId(User);
                _noteService.Fav(request.NoteId, userId);
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
        
        [HttpDelete("fav")]
        public IActionResult UnFav([FromBody] NoteUnFavRequest request)
        {
            try
            {
                var userId = _authenticationService.GetAuthenticatedUserId(User);
                _noteService.UnFav(request.NoteId,  userId);
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
        
        [HttpPost("rate")]
        public IActionResult Rate([FromBody] NoteRateRequest request)
        {
            try
            {
                var userId = _authenticationService.GetAuthenticatedUserId(User);
                var rate = _noteService.Rate(request.NoteId, request.Rate, userId);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success,
                    Rate = rate
                });
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }
    }
}