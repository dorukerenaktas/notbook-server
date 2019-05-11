using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotBook.Core.Constants;
using NotBook.Core.Models;
using NotBook.Service.Search;

namespace NotBook.Api.Controllers
{
    [Authorize]
    [Route(Constants.Version + "/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }
        
        [HttpGet]
        public IActionResult Read([FromQuery] string query)
        {
            try
            {
                var universities = _searchService.SearchUniversity(query);
                var lectures = _searchService.SearchLecture(query);

                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success,
                    Universities = universities,
                    Lectures = lectures
                });
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }
    }
}