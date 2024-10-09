using GTE.Mastery.LearnIT.Domain.Models;
using Microsoft.EntityFrameworkCore;
using GTE.Mastery.LearnIT.Data.Database;
using GTE.Mastery.LearnIT.Data.Interfaces;

namespace GTE.Mastery.LearnIT.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }
        public async Task<UserInformation?> GetUserInfoByIdAsync(int userId)
        {
            return await _context.UsersInformation.FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<IReadOnlyList<User>> GetAllUsersByCourseIdAsync(int courseId)
        {
            //  return await _context.CourseUsers
            //.Where(u => u.CourseId == courseId)
            //.Select(u => u.User)
            //.AsNoTracking()
            //.ToListAsync();

            return new List<User>();
        }
        public async Task<IReadOnlyList<User>> GetAllAsync()
        {
            return await _context.Set<User>()
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<IReadOnlyList<UserInformation>> GetAllUsersInfoAsync()
        {
            return await _context.Set<UserInformation>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await _context.Set<User>().AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(UserInformation userInformation)
        {
            if (userInformation == null)
            {
                throw new ArgumentNullException(nameof(userInformation));
            }

            await _context.Set<UserInformation>().AddAsync(userInformation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Set<User>().Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserInformation userInformation)
        {
            _context.Set<UserInformation>().Update(userInformation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int userId)
        {
            var user = await _context.Set<User>().FindAsync(userId);
            var userInfo = await _context.Set<UserInformation>().FindAsync(userId);
            if (user != null)
            {
                _context.Set<User>().Remove(user);
            }
            if (userInfo != null)
            {
                _context.Set<UserInformation>().Remove(userInfo);
            }
            await _context.SaveChangesAsync();
        }
    }
}
