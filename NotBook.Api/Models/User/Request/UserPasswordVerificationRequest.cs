namespace NotBook.Api.Models.User.Request
{
    public class UserPasswordVerificationRequest
    {
        public string Hash { get; set; }
        
        public string Password { get; set; }
    }
}