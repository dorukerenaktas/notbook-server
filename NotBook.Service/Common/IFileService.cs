using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace NotBook.Service.Common
{
    public interface IFileService
    {
        void Save(IFormFile file, string path, string name);

        FileStream Get(string path, string name);

        string GenerateFileName(IFormFile file, string context);
    }
}