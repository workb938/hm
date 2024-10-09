namespace GTE.Mastery.LearnIT.Domain.Models
{
    public class UserInformation
    {
        public int UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Email { get; set; }
        public required string LearningInstitution { get; set; }
    }
}
