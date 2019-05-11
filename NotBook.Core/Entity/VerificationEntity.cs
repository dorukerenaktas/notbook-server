using System.ComponentModel.DataAnnotations;

namespace NotBook.Core.Entity
{
    public class VerificationEntity
    {
        [Key] public int VerificationIdPk { get; set; }
        
        public int VerificationUserIdFk { get; set; }

        public string VerificationHash { get; set; }
        
        public VerificationType VerificationType { get; set; }
    }
    
    public enum VerificationType
    {
        Email = 0,
        Password = 1
    }
}