using GTE.Mastery.LearnIT.Domain.Models;

namespace GTE.Mastery.LearnIT.Domain.ViewModels
{
    public class CoursesViewModel
    {
        public IEnumerable<Course> Courses { get; set; }
        public User User { get; set; }
    }
}
