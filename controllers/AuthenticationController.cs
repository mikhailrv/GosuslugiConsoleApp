using System;
using GosuslugiWinForms.Models;
using GosuslugiWinForms.Services;

namespace GosuslugiWinForms.Controllers
{
    public class AuthenticationController
    {
        private readonly AuthenticationService _authService;

        public AuthenticationController(AuthenticationService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        public Account? Login(string login, string password)
        {
            return _authService.Login(login, password);
        }

        public User RegisterUser(string login, string password, string fullName)
        {
            return _authService.RegisterUser(login, password, fullName);
        }
    }
}