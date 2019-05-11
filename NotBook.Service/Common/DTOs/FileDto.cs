using System.IO;

namespace NotBook.Service.Common.DTOs
{
    public class FileDto
    {
        public string FileName { get; set; }
        
        public string FileExtension { get; set; }
        
        public FileStream FileStream { get; set; }
    }
}