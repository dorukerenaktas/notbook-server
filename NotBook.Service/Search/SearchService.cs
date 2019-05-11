using System.Linq;
using Dapper;
using NotBook.Core.Entity;
using NotBook.Data.MicroOrm;
using NotBook.Service.Search.DTOs;
using NotBook.Service.University.DTOs;

namespace NotBook.Service.Search
{
    public class SearchService : ISearchService
    {
        private readonly IMicroOrmRepository _microOrmRepository;
        
        public SearchService(IMicroOrmRepository microOrmRepository)
        {
            _microOrmRepository = microOrmRepository;
        }
        
        public SearchUniversityDto[] SearchUniversity(string query)
        {
            const string sql =
                "SELECT UniversityIdPk AS Id, UniversityName AS Name, UniversityAbbr As Abbr, UniversityImageUrl As ImageUrl " +
                "FROM University " +
                "WHERE UniversityName LIKE @keyword OR UniversityAbbr LIKE @keyword " + 
                "LIMIT 5";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("keyword", "%" + query + "%");
            var result = _microOrmRepository.Connection.Query<SearchUniversityDto>(sql,
                dynamicParameters).ToArray();
            
            return result;
        }

        public SearchLectureDto[] SearchLecture(string query)
        {
            const string sql =
                "SELECT  LectureIdPk, LectureCode, LectureName," +
                "UniversityIdPk, UniversityName, UniversityAbbr, UniversityImageUrl " +
                "FROM Lecture INNER JOIN University ON LectureUniversityIdFk = UniversityIdPk " +
                "WHERE LectureCode LIKE @keyword OR LectureName LIKE @keyword " + 
                "LIMIT 5";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("keyword", "%" + query + "%");
            var result = _microOrmRepository.Connection.Query<LectureEntity, UniversityEntity, SearchLectureDto>(sql,
                (lecture, university) => new SearchLectureDto
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
    }
}