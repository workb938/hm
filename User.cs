using GTE.Mastery.LearnIT.Domain.Enums;

namespace GTE.Mastery.LearnIT.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public Role Role { get; set; }
        public ICollection<CourseUser> CourseUsers { get; set; } = new List<CourseUser>();
    }
}
