using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTE.Mastery.LearnIT.Domain.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<CourseUser> CourseUsers { get; set; } = new List<CourseUser>();
    }
}
