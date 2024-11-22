using Microsoft.EntityFrameworkCore;
using ShoesStore.IRepository;
using ShoesStore.ViewModel.RequestModel;
using WebApi.Data;
using WebApi.Models;

namespace ShoesStore.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserRequestViewModel>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users.Select(user => new UserRequestViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber
            }).ToList();
        }

        public async Task<UserRequestViewModel?> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;

            return new UserRequestViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber
            };
        }

        public async Task<UserRequestViewModel?> AddUserAsync(UserRequestViewModel request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                throw new Exception("Email already exists.");

            var user = new AppUser
            {
                UserName = request.UserName,
                Email = request.Email,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserRequestViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber
            };
        }

        public async Task<UserRequestViewModel?> UpdateUserAsync(string email, UserRequestViewModel request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;

            user.FullName = request.FullName;
            user.PhoneNumber = request.PhoneNumber;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return new UserRequestViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber
            };
        }

        public async Task<bool> DeleteUserAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
