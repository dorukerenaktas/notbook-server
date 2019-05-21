using System.Linq;
using Dapper;
using NotBook.Core.Entity;
using NotBook.Data.MicroOrm;
using NotBook.Service.University.DTOs;
using NotBook.Service.University.Exception;

namespace NotBook.Service.University
{
    public class UniversityService : IUniversityService
    {
        private readonly IMicroOrmRepository _microOrmRepository;
        
        public UniversityService(IMicroOrmRepository microOrmRepository)
        {
            _microOrmRepository = microOrmRepository;
        }
        
        public UniversityDto Read(int universityId)
        {
            const string sql =
                "SELECT UniversityIdPk AS Id, UniversityName AS Name, UniversityAbbr As Abbr, UniversityImageUrl As ImageUrl, UniversityLocation AS Location, " +
                "(SELECT COUNT(*) FROM User WHERE UserUniversityIdFk = UniversityIdPk AND UserIsVerified = 1) AS StudentCount," +
                "(SELECT COUNT(*) FROM Lecture WHERE LectureUniversityIdFk = UniversityIdPk) AS LectureCount," +
                "(SELECT COUNT(*) FROM Post WHERE PostParentIdFk = UniversityIdPk AND PostType = @postType AND PostIsDeleted != true) AS CommentCount " +
                "FROM University " +
                "WHERE UniversityIdPk = @universityId ";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("universityId", universityId);
            dynamicParameters.Add("postType", PostType.University);
            var result = _microOrmRepository.Connection.Query<UniversityDto>(sql, dynamicParameters).FirstOrDefault();
            
            if (result == null)
                throw new UniversityNotFoundException();
            
            return result;
        }
    }
}