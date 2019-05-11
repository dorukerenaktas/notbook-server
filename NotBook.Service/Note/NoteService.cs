using System;
using System.IO;
using System.Linq;
using Dapper;
using Microsoft.AspNetCore.Http;
using NotBook.Core.Entity;
using NotBook.Data.Constants;
using NotBook.Data.MicroOrm;
using NotBook.Data.Repository;
using NotBook.Service.Common;
using NotBook.Service.Common.DTOs;
using NotBook.Service.Lecture.DTOs;
using NotBook.Service.Note.DTOs;
using NotBook.Service.Note.Exception;
using NotBook.Service.University.DTOs;
using NotBook.Service.User.DTOs;

namespace NotBook.Service.Note
{
    public class NoteService : INoteService
    {
        private readonly IMicroOrmRepository _microOrmRepository;
        private readonly IRepository<NoteEntity> _noteRepository;
        private readonly IRepository<NoteFavEntity> _noteFavRepository;
        private readonly IRepository<NoteRateEntity> _noteRateRepository;
        private readonly IRepository<NoteReportEntity> _noteReportRepository;
        private readonly IFileService _fileService;

        public NoteService(IMicroOrmRepository microOrmRepository,
            IRepository<NoteEntity> noteRepository,
            IRepository<NoteFavEntity> noteFavRepository,
            IRepository<NoteRateEntity> noteRateRepository,
            IRepository<NoteReportEntity> noteReportRepository,
            IFileService fileService)
        {
            _microOrmRepository = microOrmRepository;
            _noteRepository = noteRepository;
            _noteFavRepository = noteFavRepository;
            _noteRateRepository = noteRateRepository;
            _noteReportRepository = noteReportRepository;
            _fileService = fileService;
        }
        
        public int Create(int lectureId, string name, string description, NoteTag tag, IFormFile document, int createdBy)
        {
            var fileName = _fileService.GenerateFileName(document, createdBy.ToString());
            _fileService.Save(document, DataConstants.NoteFilePath, fileName);
            
            var note = new NoteEntity
            {
                NoteName = name,
                NoteDescription = description,
                NoteTag = tag,
                NoteLectureIdFk = lectureId,
                NoteCreatedByIdFk = createdBy,
                NoteFileName = fileName,
                NoteCreatedAt = DateTime.Now
            };
            
            _noteRepository.Insert(note);
            _noteRepository.SaveAll();

            return note.NoteIdPk;
        }
        
        public NoteDto[] ReadAll(int lectureId, int userId)
        {
            const string sql =
                "SELECT NoteIdPk, NoteName, NoteDescription, NoteTag, NoteIsEdited, NoteFileName, NoteCreatedAt," +
                "(SELECT COUNT(*) FROM NoteFav WHERE NoteFavNoteIdFk = NoteIdPk) AS NoteFavCount," +
                "(SELECT AVG(NoteRateValue) FROM NoteRate WHERE NoteRateNoteIdFk = NoteIdPk) AS NoteRate," +
                "(SELECT COUNT(*) FROM Comment WHERE CommentParentIdFk = NoteIdPk AND CommentType = @commentType AND CommentIsDeleted != true) AS NoteCommentCount," +
                "(SELECT EXISTS(SELECT * FROM NoteFav WHERE NoteFavUserIdFk = @userId AND NoteFavNoteIdFk = NoteIdPk)) AS NoteIsFav," +
                "LectureIdPk, LectureCode, LectureName, " +
                "UniversityIdPk, UniversityName, UniversityAbbr, UniversityImageUrl, " +
                "UserIdPk, UserFirstName, UserLastName " +
                "FROM Note INNER JOIN Lecture ON NoteLectureIdFk = LectureIdPk " +
                "INNER JOIN University ON LectureUniversityIdFk = UniversityIdPk " +
                "INNER JOIN User ON NoteCreatedByIdFk = UserIdPk " +
                "WHERE NoteLectureIdFk = @lectureId " + 
                "ORDER BY NoteRate DESC, NoteCreatedAt DESC";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("lectureId", lectureId);
            dynamicParameters.Add("userId", userId);
            dynamicParameters.Add("commentType", CommentType.Note);
            var result = _microOrmRepository.Connection.Query<ExtendedNoteEntity, LectureEntity, UniversityEntity, UserEntity, NoteDto>(sql,
                (note, lecture, university, user) => new NoteDto
                {
                    Id = note.NoteIdPk,
                    Name = note.NoteName,
                    Description = note.NoteDescription,
                    Rate = note.NoteRate,
                    Tag = (int) note.NoteTag,
                    FavCount = note.NoteFavCount,
                    CommentCount = note.NoteCommentCount,
                    FileExtension = Path.GetExtension(note.NoteFileName),
                    IsFav = note.NoteIsFav,
                    IsEdited = note.NoteIsEdited,
                    CreatedAt = note.NoteCreatedAt,
                    Lecture = new SimpleLectureDto
                    {
                        Id = lecture.LectureIdPk,
                        Code = lecture.LectureCode,
                        Name = lecture.LectureName,
                        University = new SimpleUniversityDto
                        {
                            Id = university.UniversityIdPk,
                            Name = university.UniversityName,
                            Abbr = university.UniversityAbbr,
                            ImageUrl = university.UniversityImageUrl
                        }
                    },
                    CreatedBy = new SimpleUserDto
                    {
                        Id = user.UserIdPk,
                        FirstName = user.UserFirstName,
                        LastName = user.UserLastName
                    }
                },
                dynamicParameters,
                splitOn: "LectureIdPk, UniversityIdPk, UserIdPk").ToArray();
            
            return result;
        }
        
        public NoteDto[] ReadFav(int userId)
        {
            const string sql =
                "SELECT NoteIdPk, NoteName, NoteDescription, NoteTag, NoteIsEdited, NoteFileName, NoteCreatedAt," +
                "(SELECT COUNT(*) FROM NoteFav WHERE NoteFavNoteIdFk = NoteIdPk) AS NoteFavCount," +
                "(SELECT AVG(NoteRateValue) FROM NoteRate WHERE NoteRateNoteIdFk = NoteIdPk) AS NoteRate," +
                "(SELECT COUNT(*) FROM Comment WHERE CommentParentIdFk = NoteIdPk AND CommentType = @commentType AND CommentIsDeleted != true) AS NoteCommentCount," +
                "(SELECT EXISTS(SELECT * FROM NoteFav WHERE NoteFavUserIdFk = @userId AND NoteFavNoteIdFk = NoteIdPk)) AS NoteIsFav," +
                "LectureIdPk, LectureCode, LectureName, " +
                "UniversityIdPk, UniversityName, UniversityAbbr, UniversityImageUrl, " +
                "UserIdPk, UserFirstName, UserLastName " +
                "FROM NoteFav INNER JOIN Note ON NoteFavNoteIdFk = NoteIdPk " +
                "INNER JOIN Lecture ON NoteLectureIdFk = LectureIdPk " +
                "INNER JOIN University ON LectureUniversityIdFk = UniversityIdPk " +
                "INNER JOIN User ON NoteCreatedByIdFk = UserIdPk " +
                "WHERE NoteFavUserIdFk = @userId " + 
                "ORDER BY NoteFavCreatedAt ASC";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("userId", userId);
            dynamicParameters.Add("commentType", CommentType.Note);
            var result = _microOrmRepository.Connection.Query<ExtendedNoteEntity, LectureEntity, UniversityEntity, UserEntity, NoteDto>(sql,
                (note, lecture, university, user) => new NoteDto
                {
                    Id = note.NoteIdPk,
                    Name = note.NoteName,
                    Description = note.NoteDescription,
                    Rate = note.NoteRate,
                    Tag = (int) note.NoteTag,
                    FavCount = note.NoteFavCount,
                    CommentCount = note.NoteCommentCount,
                    FileExtension = Path.GetExtension(note.NoteFileName),
                    IsFav = note.NoteIsFav,
                    IsEdited = note.NoteIsEdited,
                    CreatedAt = note.NoteCreatedAt,
                    Lecture = new SimpleLectureDto
                    {
                        Id = lecture.LectureIdPk,
                        Code = lecture.LectureCode,
                        Name = lecture.LectureName,
                        University = new SimpleUniversityDto
                        {
                            Id = university.UniversityIdPk,
                            Name = university.UniversityName,
                            Abbr = university.UniversityAbbr,
                            ImageUrl = university.UniversityImageUrl
                        }
                    },
                    CreatedBy = new SimpleUserDto
                    {
                        Id = user.UserIdPk,
                        FirstName = user.UserFirstName,
                        LastName = user.UserLastName
                    }
                },
                dynamicParameters,
                splitOn: "LectureIdPk, UniversityIdPk, UserIdPk").ToArray();
            
            return result;
        }
        
        public NoteDto[] ReadAdded(int userId)
        {
            const string sql =
                "SELECT NoteIdPk, NoteName, NoteDescription, NoteTag, NoteIsEdited, NoteFileName, NoteCreatedAt," +
                "(SELECT COUNT(*) FROM NoteFav WHERE NoteFavNoteIdFk = NoteIdPk) AS NoteFavCount," +
                "(SELECT AVG(NoteRateValue) FROM NoteRate WHERE NoteRateNoteIdFk = NoteIdPk) AS NoteRate," +
                "(SELECT COUNT(*) FROM Comment WHERE CommentParentIdFk = NoteIdPk AND CommentType = @commentType AND CommentIsDeleted != true) AS NoteCommentCount," +
                "(SELECT EXISTS(SELECT * FROM NoteFav WHERE NoteFavUserIdFk = @userId AND NoteFavNoteIdFk = NoteIdPk)) AS NoteIsFav," +
                "LectureIdPk, LectureCode, LectureName, " +
                "UniversityIdPk, UniversityName, UniversityAbbr, UniversityImageUrl, " +
                "UserIdPk, UserFirstName, UserLastName " +
                "FROM Note INNER JOIN Lecture ON NoteLectureIdFk = LectureIdPk " +
                "INNER JOIN University ON LectureUniversityIdFk = UniversityIdPk " +
                "INNER JOIN User ON NoteCreatedByIdFk = UserIdPk " +
                "WHERE NoteCreatedByIdFk = @userId " + 
                "ORDER BY NoteCreatedAt ASC";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("userId", userId);
            dynamicParameters.Add("commentType", CommentType.Note);
            var result = _microOrmRepository.Connection.Query<ExtendedNoteEntity, LectureEntity, UniversityEntity, UserEntity, NoteDto>(sql,
                (note, lecture, university, user) => new NoteDto
                {
                    Id = note.NoteIdPk,
                    Name = note.NoteName,
                    Description = note.NoteDescription,
                    Rate = note.NoteRate,
                    Tag = (int) note.NoteTag,
                    FavCount = note.NoteFavCount,
                    CommentCount = note.NoteCommentCount,
                    FileExtension = Path.GetExtension(note.NoteFileName),
                    IsFav = note.NoteIsFav,
                    IsEdited = note.NoteIsEdited,
                    CreatedAt = note.NoteCreatedAt,
                    Lecture = new SimpleLectureDto
                    {
                        Id = lecture.LectureIdPk,
                        Code = lecture.LectureCode,
                        Name = lecture.LectureName,
                        University = new SimpleUniversityDto
                        {
                            Id = university.UniversityIdPk,
                            Name = university.UniversityName,
                            Abbr = university.UniversityAbbr,
                            ImageUrl = university.UniversityImageUrl
                        }
                    },
                    CreatedBy = new SimpleUserDto
                    {
                        Id = user.UserIdPk,
                        FirstName = user.UserFirstName,
                        LastName = user.UserLastName
                    }
                },
                dynamicParameters,
                splitOn: "LectureIdPk, UniversityIdPk, UserIdPk").ToArray();
            
            return result;
        }
        
        public FileDto ReadDocument(int noteId)
        {
            var note = _noteRepository.Table.FirstOrDefault(x => x.NoteIdPk == noteId);
            if (note == null)
                throw new NoteNotFoundException();
            
            var stream = _fileService.Get(DataConstants.NoteFilePath, note.NoteFileName);
            return new FileDto
            {
                FileName = note.NoteName,
                FileExtension = Path.GetExtension(note.NoteFileName),
                FileStream = stream
            };
        }
        
        public void Update(int noteId, string name, string description, NoteTag tag, int userId)
        {
            var note = _noteRepository.Table.FirstOrDefault(x => 
                x.NoteIdPk == noteId && x.NoteCreatedByIdFk == userId);
            
            if (note == null)
                throw new NoteNotFoundException();

            note.NoteName = name;
            note.NoteDescription = description;
            note.NoteTag = tag;
            note.NoteIsEdited = true;
            
            _noteRepository.SaveAll();
        }
        
        public void Report(int noteId, int userId)
        {
            if (_noteReportRepository.Table.Any(x =>
                x.NoteReportNoteIdFk == noteId && x.NoteReportUserIdFk == userId))
                throw new NoteAlreadyReportedException();
            
            var report = new NoteReportEntity
            {
                NoteReportNoteIdFk = noteId,
                NoteReportUserIdFk = userId,
                NoteReportCreatedAt = DateTime.Now
            };
           
            _noteReportRepository.Insert(report);
            _noteReportRepository.SaveAll();
        }
        
        public void Fav(int noteId, int userId)
        {
            if (_noteFavRepository.Table.Any(x =>
                x.NoteFavNoteIdFk == noteId && x.NoteFavUserIdFk == userId))
                throw new NoteAlreadyFavException();

            var fav = new NoteFavEntity
            {
                NoteFavNoteIdFk = noteId,
                NoteFavUserIdFk = userId,
                NoteFavCreatedAt = DateTime.Now
            };
            
            _noteFavRepository.Insert(fav);
            _noteFavRepository.SaveAll();
        }
        
        public void UnFav(int noteId, int userId)
        {
            var fav = _noteFavRepository.Table.FirstOrDefault(x =>
                x.NoteFavNoteIdFk == noteId && x.NoteFavUserIdFk == userId);
            
            if (fav == null)
                throw new NoteNotFavException();
            
            _noteFavRepository.Delete(fav);
            _noteFavRepository.SaveAll();
        }
        
        public float Rate(int noteId, float value, int userId)
        {
            var rate = _noteRateRepository.Table.FirstOrDefault(x =>
                x.NoteRateNoteIdFk == noteId && x.NoteRateUserIdFk == userId);

            if (rate == null)
            {
                rate = new NoteRateEntity
                {
                    NoteRateNoteIdFk = noteId,
                    NoteRateUserIdFk = userId,
                    NoteRateValue = value,
                    NoteRateCreatedAt = DateTime.Now
                };
                
                _noteRateRepository.Insert(rate);
            }
            else
            {
                rate.NoteRateValue = value;
            }
                
            _noteRateRepository.SaveAll();
            
            return _noteRateRepository.Table.Where(x => x.NoteRateNoteIdFk == noteId).ToList().Average(x => x.NoteRateValue);
        }
    }
}