using System.ComponentModel.DataAnnotations;

namespace NotBook.Core.Entity
{
    public class UniversityEntity
    {
        [Key] public int UniversityIdPk { get; set; }

        public string UniversityName { get; set; }

        public string UniversityAbbr { get; set; }

        public string UniversityMailExtension { get; set; }

        public string UniversityImageUrl { get; set; }
        
        public int UniversityLocation { get; set; }
    }
}