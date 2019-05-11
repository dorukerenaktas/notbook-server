using Microsoft.AspNetCore.Http;

namespace NotBook.Api.Models.User.Request
{
    public class UserUpdateImageRequest
    {
        public IFormFile Image { get; set; }
    }
}