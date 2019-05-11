using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotBook.Core.Constants;
using NotBook.Core.Models;
using NotBook.Service.University;
using NotBook.Service.University.Exception;

namespace NotBook.Api.Controllers
{
    [Authorize]
    [Route(Constants.Version + "/[controller]")]
    [ApiController]
    public class UniversityController : ControllerBase
    {
        private readonly IUniversityService _universityService;

        public UniversityController(IUniversityService universityService)
        {
            _universityService = universityService;
        }
        
        [HttpGet]
        public IActionResult Read([FromQuery] int universityId)
        {
            try
            {
                var university = _universityService.Read(universityId);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success,
                    Data = university
                });
            }
            catch (UniversityNotFoundException)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.NotFound});
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }
    }
}