using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using NotBook.Data.Constants;

namespace NotBook.Service.Common
{
    public class FileService : IFileService
    {        
        public void Save(IFormFile file, string path, string name)
        {
            var root = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var directory = Path.Combine(root, Path.Combine(DataConstants.BaseFilePath, path));
            if (file.Length > 0) {
                var filePath = Path.Combine(directory, name);
                using (var fileStream = new FileStream(filePath, FileMode.Create)) {
                    file.CopyTo(fileStream);
                }
            }
        }
        
        public FileStream Get(string path, string name)
        {
            var root = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var directory = Path.Combine(root, Path.Combine(DataConstants.BaseFilePath, path));
            var filePath = Path.Combine(directory, name);
            return File.OpenRead(filePath);
        }
        
        public string GenerateFileName(IFormFile file, string context)
        {
            return context + "_" +
                   DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" +
                   Guid.NewGuid().ToString("N") + 
                   Path.GetExtension(file.FileName);
        }
    }
}