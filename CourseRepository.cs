using GTE.Mastery.LearnIT.Domain.Models;
using Microsoft.EntityFrameworkCore;
using GTE.Mastery.LearnIT.Data.Database;
using GTE.Mastery.LearnIT.Data.Repositories.Interfaces;
using GTE.Mastery.LearnIT.Domain.ViewModels;

namespace GTE.Mastery.LearnIT.Data.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly AppDbContext _context;

    public CourseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Course?> GetByIdAsync(int courseId)
    {
        return await _context.Set<Course>()
            .FirstOrDefaultAsync(o => o.Id == courseId);
    }

    public async Task AddRelationAsync(int? courseId, int? userId)
    {
        if (userId == null)
        {
            throw new ArgumentNullException(nameof(userId));
        }

        if (courseId == null)
        {
            throw new ArgumentNullException(nameof(courseId));
        }
        
        var exists = await _context.CourseUsers.AnyAsync(cu => cu.CourseId == courseId && cu.UserId == userId);
        if (!exists)
        {
            await _context.CourseUsers.AddAsync(new CourseUser { CourseId = (int)courseId, UserId = (int)userId });
            await _context.SaveChangesAsync();
        }
    }

    public async Task<CoursesViewModel> GetAllCoursesByUserIdAsync(int userId)
    {
        var courses = await _context.CourseUsers
            .Where(u => u.UserId == userId)
            .Select(u => u.Course)
            .AsNoTracking()
            .ToListAsync();

        var user = await _context.Users
               .Where(u => u.Id == userId)
               .Select(u => new User
               {
                   Id = u.Id,
                   UserName = u.UserName,
                   PasswordHash = u.PasswordHash,
                   Role = u.Role
               })
               .FirstOrDefaultAsync();


        var courseViewModel = new CoursesViewModel
        {
            Courses = courses,
            User = user
        };

        return courseViewModel;
    }

    public async Task<IReadOnlyList<Course>> GetAllAsync()
    {
        return await _context.Set<Course>().AsNoTracking().ToListAsync();
    }

    public async Task<Course> AddAsync(Course course)
    {
        await _context.Set<Course>().AddAsync(course);
        await _context.SaveChangesAsync();
        return course;
    }

    public async Task UpdateAsync(Course course)
    {
        _context.Set<Course>().Update(course);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int courseId)
    {
        var course = await _context.Set<Course>().FindAsync(courseId);
        if (course != null)
        {
            _context.Set<Course>().Remove(course);
            await _context.SaveChangesAsync();
        }
    }
}
