using Microsoft.EntityFrameworkCore;
using NotBook.Core.Entity;

namespace NotBook.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserEntity>(x => { x.ToTable("User"); });
            builder.Entity<VerificationEntity>(x => { x.ToTable("Verification"); });
            builder.Entity<UniversityEntity>(x => { x.ToTable("University"); });
            builder.Entity<LectureEntity>(x => { x.ToTable("Lecture"); });
            builder.Entity<AttendEntity>(x => { x.ToTable("Attend"); });
            builder.Entity<PostEntity>(x => { x.ToTable("Post"); });
            builder.Entity<PostLikeEntity>(x => { x.ToTable("PostLike"); });
            builder.Entity<PostReportEntity>(x => { x.ToTable("PostReport"); });
            builder.Entity<NoteEntity>(x => { x.ToTable("Note"); }); 
            builder.Entity<NoteFavEntity>(x => { x.ToTable("NoteFav"); }); 
            builder.Entity<NoteRateEntity>(x => { x.ToTable("NoteRate"); }); 
            builder.Entity<NoteReportEntity>(x => { x.ToTable("NoteReport"); }); 
            builder.Entity<CommentEntity>(x => { x.ToTable("Comment"); });
            builder.Entity<CommentLikeEntity>(x => { x.ToTable("CommentLike"); });
            builder.Entity<CommentReportEntity>(x => { x.ToTable("CommentReport"); });
            base.OnModelCreating(builder);
        }
    }
}