namespace NotBook.Api.Models.User.Request
{
    public class UserRefreshTokenRequest
    {
        public string AccessToken { get; set; }
        
        public string RefreshToken { get; set; }
    }
}