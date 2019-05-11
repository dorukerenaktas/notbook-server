using Microsoft.AspNetCore.Http;
using NotBook.Service.Common.DTOs;
using NotBook.Service.User.DTOs;

namespace NotBook.Service.User
{
    public interface IUserService
    {
        UserDto Authenticate(string email, string password);

        void Create(string firstName, string lastName, string email, string password);

        UserDto Read(int id);

        void Update(int id, string firstName, string lastName, string password);

        FileDto ReadImage(int userId);

        void UpdateImage(IFormFile file, int userId);
        
        void VerifyEmail(string verificationHash);

        void ResendEmailVerificationEmail(string email);
        
        void VerifyPassword(string verificationHash, string password);

        void SendForgotPasswordEmail(string email);
    }
}