using System.Linq;
using Dapper;
using NotBook.Core.Entity;
using NotBook.Data.MicroOrm;
using NotBook.Data.Repository;
using NotBook.Service.Lecture.DTOs;
using NotBook.Service.Lecture.Exception;
using NotBook.Service.University.DTOs;
using NotBook.Service.User.Exception;

namespace NotBook.Service.Lecture
{
    public class LectureService : ILectureService
    {
        private readonly IMicroOrmRepository _microOrmRepository;
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IRepository<LectureEntity> _lectureRepository;
        private readonly IRepository<AttendEntity> _attendRepository;

        public LectureService(IMicroOrmRepository microOrmRepository,
            IRepository<UserEntity> userRepository,
            IRepository<LectureEntity> lectureRepository,
            IRepository<AttendEntity> attendRepository)
        {
            _microOrmRepository = microOrmRepository;
            _userRepository = userRepository;
            _lectureRepository = lectureRepository;
            _attendRepository = attendRepository;
        }
        
        public int Create(string code, string name, int universityId, int createdBy)
        {            
            if (_lectureRepository.Table.Any(x =>
                (x.LectureCode == code || x.LectureName == name) && x.LectureUniversityIdFk == universityId))
                throw new LectureAlreadyExistsException();
            
            if (!_userRepository.Table.Any(x =>
                x.UserUniversityIdFk == universityId && x.UserIdPk == createdBy))
                throw new UserNotFoundException();

            var lecture = new LectureEntity
            {
                LectureCode = code,
                LectureName = name,
                LectureUniversityIdFk = universityId,
                LectureCreatedByIdFk = createdBy
            };
            
            _lectureRepository.Insert(lecture);
            _lectureRepository.SaveAll();

            return lecture.LectureIdPk;
        }

        public LectureDto Read(int lectureId, int userId)
        {
            const string sql =
                "SELECT LectureIdPk, LectureCode, LectureName," +
                "(SELECT COUNT(*) FROM Attend WHERE AttendLectureIdFk = @lectureId) AS LectureAttendCount," +
                "(SELECT COUNT(*) FROM Post WHERE PostParentIdFk = @lectureId AND PostIsDeleted != true) AS LecturePostCount," +
                "(SELECT COUNT(*) FROM Note WHERE NoteLectureIdFk = @lectureId) AS LectureNoteCount," +
                "(SELECT EXISTS(SELECT * FROM Attend WHERE AttendUserIdFk = @userId AND AttendLectureIdFk = @lectureId)) AS LectureIsAttended," +
                "UniversityIdPk, UniversityName, UniversityAbbr, UniversityImageUrl " +
                "FROM Lecture INNER JOIN University ON LectureUniversityIdFk = UniversityIdPk " +
                "WHERE LectureIdPk = @lectureId";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("lectureId", lectureId);
            dynamicParameters.Add("userId", userId);
            var result = _microOrmRepository.Connection.Query<ExtendedLectureEntity, UniversityEntity, LectureDto>(sql,
                (lecture, university) => new LectureDto
                {
                    Id = lecture.LectureIdPk,
                    Code = lecture.LectureCode,
                    Name = lecture.LectureName,
                    AttendCount = lecture.LectureAttendCount,
                    PostCount = lecture.LecturePostCount,
                    NoteCount = lecture.LectureNoteCount,
                    IsAttended = lecture.LectureIsAttended,
                    University = new SimpleUniversityDto
                    {
                        Id = university.UniversityIdPk,
                        Name = university.UniversityName,
                        Abbr = university.UniversityAbbr,
                        ImageUrl = university.UniversityImageUrl
                    }
                },
                dynamicParameters,
                splitOn: "UniversityIdPk").FirstOrDefault();

            if (result == null)
                throw new LectureNotFoundException();

            return result;
        }
        
        public SimpleLectureDto[] ReadAll(int universityId)
        {
            const string sql =
                "SELECT  LectureIdPk, LectureCode, LectureName," +
                "UniversityIdPk, UniversityName, UniversityAbbr, UniversityImageUrl " +
                "FROM Lecture INNER JOIN University ON LectureUniversityIdFk = UniversityIdPk " +
                "WHERE LectureUniversityIdFk = @universityId ";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("universityId", universityId);
            var result = _microOrmRepository.Connection.Query<LectureEntity, UniversityEntity, SimpleLectureDto>(sql,
                (lecture, university) => new SimpleLectureDto
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
                dynamicParameters,
                splitOn: "UniversityIdPk").ToArray();
            
            return result;
        }
        
        public SimpleLectureDto[] ReadAttended (int userId)
        {
            const string sql =
                "SELECT  LectureIdPk, LectureCode, LectureName," +
                "UniversityIdPk, UniversityName, UniversityAbbr, UniversityImageUrl " +
                "FROM Lecture INNER JOIN University ON LectureUniversityIdFk = UniversityIdPk INNER JOIN Attend ON AttendLectureIdFk = LectureIdPk " +
                "WHERE AttendUserIdFk = @userId ";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("userId", userId);
            var result = _microOrmRepository.Connection.Query<LectureEntity, UniversityEntity, SimpleLectureDto>(sql,
                (lecture, university) => new SimpleLectureDto
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
                dynamicParameters,
                splitOn: "UniversityIdPk").ToArray();
            
            return result;
        }
        
        public SimpleLectureDto[] ReadAdded (int userId)
        {
            const string sql =
                "SELECT  LectureIdPk, LectureCode, LectureName," +
                "UniversityIdPk, UniversityName, UniversityAbbr, UniversityImageUrl " +
                "FROM Lecture INNER JOIN University ON LectureUniversityIdFk = UniversityIdPk " +
                "WHERE LectureCreatedByIdFk = @userId ";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("userId", userId);
            var result = _microOrmRepository.Connection.Query<LectureEntity, UniversityEntity, SimpleLectureDto>(sql,
                (lecture, university) => new SimpleLectureDto
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
                dynamicParameters,
                splitOn: "UniversityIdPk").ToArray();
            
            return result;
        }

        public void Attend(int lectureId, int userId)
        {            
            if (_attendRepository.Table.Any(x =>
                x.AttendLectureIdFk == lectureId && x.AttendUserIdFk == userId))
                throw new UserAlreadyAttendedException();

            var attend = new AttendEntity
            {
                AttendLectureIdFk = lectureId,
                AttendUserIdFk = userId,
            };
            
            _attendRepository.Insert(attend);
            _attendRepository.SaveAll();
        }
        
        public void Quit(int lectureId, int userId)
        {
            var attend = _attendRepository.Table.FirstOrDefault(x =>
                x.AttendLectureIdFk == lectureId && x.AttendUserIdFk == userId);
            
            if (attend == null)
                throw new UserNotAttendedException();

            _attendRepository.Delete(attend);
            _attendRepository.SaveAll();
        }        
    }
}