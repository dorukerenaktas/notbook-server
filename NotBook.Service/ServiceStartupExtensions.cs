using Microsoft.Extensions.DependencyInjection;
using NotBook.Service.Authentication;
using NotBook.Service.Comment;
using NotBook.Service.Common;
using NotBook.Service.Email;
using NotBook.Service.Hash;
using NotBook.Service.Lecture;
using NotBook.Service.Note;
using NotBook.Service.Post;
using NotBook.Service.Search;
using NotBook.Service.University;
using NotBook.Service.User;

namespace NotBook.Service
{
    public static class ServiceStartupExtensions
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServiceServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, JwtAuthenticationService>();
            services.AddScoped<IHashService, HmacHashService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILectureService, LectureService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IUniversityService, UniversityService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IImageService, ImageService>();
        }
    }
}