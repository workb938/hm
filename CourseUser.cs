namespace GTE.Mastery.LearnIT.Domain.Models
{
    public class CourseUser
    {
        public int CourseId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public Course Course { get; set; }
    }
}
