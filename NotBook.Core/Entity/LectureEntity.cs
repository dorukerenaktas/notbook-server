using System;
using System.ComponentModel.DataAnnotations;

namespace NotBook.Core.Entity
{
    public class LectureEntity
    {
        [Key] public int LectureIdPk { get; set; }
        
        public string LectureCode { get; set; }

        public string LectureName { get; set; }
        
        public int LectureUniversityIdFk { get; set; }
        
        public int LectureCreatedByIdFk { get; set; }
        
        public DateTime LectureCreatedAt { get; set; }
    }

    public class ExtendedLectureEntity
    {
        [Key] public int LectureIdPk { get; set; }
        
        public string LectureCode { get; set; }

        public string LectureName { get; set; }
        
        public int LectureAttendCount { get; set; }
        
        public int LecturePostCount { get; set; }
        
        public int LectureNoteCount { get; set; }
        
        public bool LectureIsAttended { get; set; }
        
        public int LectureUniversityIdFk { get; set; }
        
        public int LectureCreatedByIdFk { get; set; }
        
        public DateTime LectureCreatedAt { get; set; }
    }
}