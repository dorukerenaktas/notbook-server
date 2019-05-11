namespace NotBook.Service.Email
{
    public interface IEmailService
    {
        void SendVerificationEmail(string email, string verificationHash);
        
        void SendForgotPasswordEmail(string email, string verificationHash);
    }
}