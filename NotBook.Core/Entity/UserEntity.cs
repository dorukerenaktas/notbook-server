using System;
using System.ComponentModel.DataAnnotations;

namespace NotBook.Core.Entity
{
    public class UserEntity
    {
        [Key] public int UserIdPk { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }
        
        public string UserImageName { get; set; }

        public string UserEmail { get; set; }

        public int UserUniversityIdFk { get; set; }

        public bool UserIsVerified { get; set; }

        public byte[] UserPasswordHash { get; set; }

        public byte[] UserPasswordSalt { get; set; }

        public DateTime UserCreatedAt { get; set; }
    }
    
    public class ExtendedUserEntity
    {
        [Key] public int UserIdPk { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }
        
        public string UserImageName { get; set; }

        public string UserEmail { get; set; }

        public int UserUniversityIdFk { get; set; }

        public bool UserIsVerified { get; set; }

        public byte[] UserPasswordHash { get; set; }

        public byte[] UserPasswordSalt { get; set; }
        
        public int UserAttendedLectureCount { get; set; }
        
        public int UserAddedLectureCount { get; set; }
        
        public int UserFavNoteCount { get; set; }

        public int UserAddedNoteCount { get; set; }

        public DateTime UserCreatedAt { get; set; }
    }
}