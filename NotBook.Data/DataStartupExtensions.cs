using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NotBook.Core.Entity;
using NotBook.Data.Constants;
using NotBook.Data.MicroOrm;
using NotBook.Data.Repository;

namespace NotBook.Data
{
    public static class DataStartupExtensions
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureDataServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository<UserEntity>, BaseRepository<UserEntity>>();
            services.AddScoped<IRepository<VerificationEntity>, BaseRepository<VerificationEntity>>();
            services.AddScoped<IRepository<UniversityEntity>, BaseRepository<UniversityEntity>>();
            services.AddScoped<IRepository<LectureEntity>, BaseRepository<LectureEntity>>();
            services.AddScoped<IRepository<AttendEntity>, BaseRepository<AttendEntity>>();
            services.AddScoped<IRepository<PostEntity>, BaseRepository<PostEntity>>();
            services.AddScoped<IRepository<PostLikeEntity>, BaseRepository<PostLikeEntity>>();
            services.AddScoped<IRepository<PostReportEntity>, BaseRepository<PostReportEntity>>();
            services.AddScoped<IRepository<NoteEntity>, BaseRepository<NoteEntity>>();
            services.AddScoped<IRepository<NoteFavEntity>, BaseRepository<NoteFavEntity>>();
            services.AddScoped<IRepository<NoteRateEntity>, BaseRepository<NoteRateEntity>>();
            services.AddScoped<IRepository<NoteReportEntity>, BaseRepository<NoteReportEntity>>();
            services.AddScoped<IRepository<CommentEntity>, BaseRepository<CommentEntity>>();
            services.AddScoped<IRepository<CommentLikeEntity>, BaseRepository<CommentLikeEntity>>();
            services.AddScoped<IRepository<CommentReportEntity>, BaseRepository<CommentReportEntity>>();
            services.AddScoped<IMicroOrmRepository, DapperRepository>();
            
            ConfigureDataEnvironment();
        }
        
        private static void ConfigureDataEnvironment()
        {
            var root = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var path = Path.Combine(root, DataConstants.BaseFilePath);
            Directory.CreateDirectory(path);
            Directory.CreateDirectory(Path.Combine(path, DataConstants.AvatarFilePath));
            Directory.CreateDirectory(Path.Combine(path, DataConstants.NoteFilePath));
        }
    }
}