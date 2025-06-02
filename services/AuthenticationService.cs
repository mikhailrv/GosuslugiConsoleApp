using GosuslugiWinForms.Models;
using GosuslugiWinForms.Repositories;


namespace GosuslugiWinForms.Services
{
    public class AuthenticationService
    {
        private readonly UserRepository _userRepository;

        public AuthenticationService(UserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public Account? Login(string login, string password)
        {
            try
            {
                var account = _userRepository.FindAll()
                    .FirstOrDefault(a => a.Login == login);

                if (account == null || !BCrypt.Net.BCrypt.Verify(password, account.Password))
                {
                    return null;
                }

                if (account is User user && !user.IsActive)
                {
                    throw new InvalidOperationException("Учетная запись не активирована.");
                }

                return account;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка при входе: {ex.Message}", ex);
            }
        }

        public User RegisterUser(string login, string password, string fullName)
        {
            try
            {
                if (_userRepository.FindAll().Any(a => a.Login == login))
                {
                    throw new InvalidOperationException("Пользователь с таким логином уже существует.");
                }

                var user = new User
                {
                    Login = login,
                    Password = BCrypt.Net.BCrypt.HashPassword(password),
                    FullName = fullName,
                    Role = Role.CITIZEN,
                    IsActive = false
                };

                _userRepository.Save(user);
                return user;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка при регистрации: {ex.Message}", ex);
            }
        }
    }
}