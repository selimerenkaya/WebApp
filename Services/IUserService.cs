using System.Collections.Generic;
using System.Threading.Tasks;
using ChatForLife.Models.Entities;

namespace ChatForLife.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> AuthenticateAsync(string username, string password);
        Task<User> RegisterUserAsync(string username, string email, string password, string fullName, DateTime birthDate);
        Task UpdateUserAsync(User user);
        Task<IEnumerable<Activity>> GetUserActivitiesAsync(int userId);
    }
}