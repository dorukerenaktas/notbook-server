namespace NotBook.Core.Constants
{
    public static class Constants
    {
        public const string Version = "v1";
        public const int MinimumPasswordLength = 8;
    }

    public static class ResponseConstants
    {
        // Default response codes
        public const int Success = 1000;
        public const int Unknown = 1001;
        public const int NotFound = 1002;
        public const int Unauthorized = 1003;

        // User response codes
        public const int InvalidStudentMail = 2001;
        public const int EmailIsTaken = 2002;
        public const int EmailOrPasswordWrong = 2003;
        public const int UserNotVerified = 2004;
        public const int UserNotExist = 2005;
        
        // Lecture response codes
        public const int LectureNotFound = 3001;
        public const int LectureAlreadyExist = 3002;
        
        public const int PostAlreadyReported = 4001;
        public const int PostIsVerified = 4002;
        
        public const int NoteAlreadyReported = 5001;
        public const int NoteIsVerified = 5002;
        
        public const int CommentAlreadyReported = 6001;
        public const int CommentIsVerified = 6002;
    }
}