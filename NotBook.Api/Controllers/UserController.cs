using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotBook.Api.Models.User.Request;
using NotBook.Core.Constants;
using NotBook.Core.Models;
using NotBook.Service.Authentication;
using NotBook.Service.Authentication.Exception;
using NotBook.Service.User;
using NotBook.Service.User.Exception;

namespace NotBook.Api.Controllers
{
    [Authorize]
    [Route(Constants.Version + "/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;

        public UserController(IUserService userService, IAuthenticationService authenticationService)
        {
            _userService = userService;
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterRequest request)
        {
            try
            {
                _userService.Create(request.FirstName, request.LastName, request.Email, request.Password);
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Success});
            }
            catch (InvalidStudentMailException)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.InvalidStudentMail});
            }
            catch (EmailIsTakenException)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.EmailIsTaken});
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginRequest request)
        {
            try
            {
                var user = _userService.Authenticate(request.Email, request.Password);
                var accessToken = _authenticationService.GenerateAccessToken(user.Id, user.University.Id, out var refreshToken);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success,
                    User = user,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                });
            }
            catch (UserNotFoundException)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.EmailOrPasswordWrong});
            }
            catch (UserNotVerifiedException)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.UserNotVerified});
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }

        [AllowAnonymous]
        [HttpGet("image")]
        public IActionResult ReadImage([FromQuery] int userId)
        {    
            try
            {
                var avatar = _userService.ReadImage(userId);
                return File(avatar.FileStream, "application/octet-stream", avatar.FileName + avatar.FileExtension);
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }
        
        [HttpPut("image")]
        public IActionResult UpdateImage([FromForm] UserUpdateImageRequest request)
        {    
            try
            {
                var userId = _authenticationService.GetAuthenticatedUserId(User);
                _userService.UpdateImage(request.Image, userId);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success
                });
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }
        
        [AllowAnonymous]
        [HttpPut("token")]
        public IActionResult RefreshToken([FromBody] UserRefreshTokenRequest request)
        {
            try
            {
                var accessToken = _authenticationService.RefreshAccessToken(request.AccessToken, request.RefreshToken, out var newRefreshToken);
                return new ObjectResult(new
                {
                    StatusCode = ResponseConstants.Success,
                    AccessToken = accessToken,
                    RefreshToken = newRefreshToken
                });
            }
            catch (TokenCanNotBeRefreshedException)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unauthorized});
            }
            catch (RefreshTokenNotValidException)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unauthorized});
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }

        [AllowAnonymous]
        [HttpPost("verify/email")]
        public IActionResult VerifyEmail([FromBody] UserEmailVerificationRequest request)
        {
            try
            {
                _userService.VerifyEmail(request.Hash);
                return new ObjectResult(new {StatusCode = ResponseConstants.Success});
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }

        [AllowAnonymous]
        [HttpPut("send/verification")]
        public IActionResult ResendVerification([FromBody] UserResendVerificationRequest request)
        {
            try
            {
                _userService.ResendEmailVerificationEmail(request.Email);
                return new ObjectResult(new {StatusCode = ResponseConstants.Success});
            }
            catch (UserNotFoundException)
            {
                return new ObjectResult(new {StatusCode = ResponseConstants.UserNotExist});
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }

        [AllowAnonymous]
        [HttpPost("verify/password")]
        public IActionResult VerifyPassword([FromBody] UserPasswordVerificationRequest request)
        {
            try
            {
                _userService.VerifyPassword(request.Hash, request.Password);
                return new ObjectResult(new {StatusCode = ResponseConstants.Success});
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }

        [AllowAnonymous]
        [HttpPut("send/password")]
        public IActionResult ForgotPassword([FromBody] UserForgotPasswordRequest request)
        {
            try
            {
                _userService.SendForgotPasswordEmail(request.Email);
                return new ObjectResult(new {StatusCode = ResponseConstants.Success});
            }
            catch (UserNotFoundException)
            {
                return new ObjectResult(new {StatusCode = ResponseConstants.UserNotExist});
            }
            catch (Exception)
            {
                return new ObjectResult(new Result {StatusCode = ResponseConstants.Unknown});
            }
        }
    }
}