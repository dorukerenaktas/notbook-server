using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace NotBook.Service.Common
{
    public class ImageService : IImageService
    {
        public IFormFile Resize(IFormFile file)
        {
            var image = Image.Load(file.OpenReadStream(),  out var format);
            
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(240, 240),
                Mode = ResizeMode.Crop
            }));

            var ms = new MemoryStream();
            image.SaveAsJpeg(ms);
            return new FormFile(ms, 0, ms.Length, file.Name, file.FileName);
        }
    }
}