using Microsoft.AspNetCore.Http;

namespace NotBook.Service.Common
{
    public interface IImageService
    {
        IFormFile Resize(IFormFile file);
    }
}