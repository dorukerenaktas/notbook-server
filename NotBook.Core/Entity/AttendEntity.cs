using System;
using System.ComponentModel.DataAnnotations;

namespace NotBook.Core.Entity
{
    public class AttendEntity
    {
        [Key] public int AttendIdPk { get; set; }
        
        public int AttendUserIdFk { get; set; }

        public int AttendLectureIdFk { get; set; }
        
        public DateTime AttendCreatedAt { get; set; }
    }
}