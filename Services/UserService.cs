using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using ChatForLife.Models.Entities;
using ChatForLife.Repositories;

namespace ChatForLife.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRepository<Activity> _activityRepository;

        public UserService(IUserRepository userRepository, IRepository<Activity> activityRepository)
        {
            _userRepository = userRepository;
            _activityRepository = activityRepository;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<bool> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null) return false;

            var passwordHash = HashPassword(password);
            return user.PasswordHash == passwordHash;
        }

        public async Task<User> RegisterUserAsync(string username, string email, string password, string fullName, DateTime birthDate)
        {
            if (await _userRepository.IsUsernameExistsAsync(username))
                throw new InvalidOperationException("Bu kullanıcı adı zaten alınmış.");

            if (await _userRepository.IsEmailExistsAsync(email))
                throw new InvalidOperationException("Bu e-posta adresi zaten kayıtlı.");

            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = HashPassword(password),
                FullName = fullName,
                BirthDate = birthDate,
                RegistrationDate = DateTime.Now
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            // Kayıt aktivitesi oluştur
            var activity = new Activity
            {
                UserId = user.Id,
                Type = "Register",
                Description = "Siteye üye oldu",
                Icon = "👋",
                Timestamp = DateTime.Now
            };

            await _activityRepository.AddAsync(activity);
            await _activityRepository.SaveChangesAsync();

            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            // Profil güncelleme aktivitesi
            var activity = new Activity
            {
                UserId = user.Id,
                Type = "ProfileUpdate",
                Description = "Profil bilgilerini güncelledi",
                Icon = "📝",
                Timestamp = DateTime.Now
            };

            await _activityRepository.AddAsync(activity);
            await _activityRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Activity>> GetUserActivitiesAsync(int userId)
        {
            return await _activityRepository.FindAsync(a => a.UserId == userId);
        }

        // Basit bir hash fonksiyonu (gerçek projede BCrypt veya PBKDF2 gibi güvenli bir hash algoritması kullanılmalıdır)
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}