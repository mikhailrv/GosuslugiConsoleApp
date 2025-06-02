using GosuslugiWinForms.Controllers;
using GosuslugiWinForms.Infrastructure;
using GosuslugiWinForms.Models;
using Microsoft.Extensions.DependencyInjection;

namespace GosuslugiWinForms.UI
{
    public partial class LoginForm : Form
    {
        private readonly AuthenticationController _authController;
        private readonly IServiceProvider _serviceProvider;
        private bool _isRegisterMode;
        private readonly TextBox _loginTextBox = new TextBox(); // Инициализация при объявлении
        private readonly TextBox _passwordTextBox = new TextBox();
        private readonly TextBox _fullNameTextBox = new TextBox();

        public LoginForm()
        {
            try
            {
                _serviceProvider = DependencyInjection.ConfigureServices();
                _authController = _serviceProvider.GetService<AuthenticationController>()
                    ?? throw new InvalidOperationException("AuthenticationController not found.");
                _isRegisterMode = false;
                InitializeUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при инициализации формы: {ex.Message}", "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void InitializeUI()
        {
            try
            {

                Text = "ТипаГосУслуги";
                Size = new Size(400, 300);
                FormBorderStyle = FormBorderStyle.FixedSingle;
                MaximizeBox = false;

                var titleLabel = new Label
                {
                    Text = "",
                    Font = new Font("Arial", 16, FontStyle.Bold),
                    AutoSize = false,
                    Size = new Size(300, 40),
                    Location = new Point(20, 10)
                };
                titleLabel.Paint += DrawTitleLabel;

                _loginTextBox.Location = new Point(100, 60);
                _loginTextBox.Size = new Size(200, 20);
                _loginTextBox.Name = "loginTextBox";

                _passwordTextBox.Location = new Point(100, 90);
                _passwordTextBox.Size = new Size(200, 20);
                _passwordTextBox.Name = "passwordTextBox";
                _passwordTextBox.PasswordChar = '*';

                _fullNameTextBox.Location = new Point(100, 120);
                _fullNameTextBox.Size = new Size(200, 20);
                _fullNameTextBox.Name = "fullNameTextBox";
                _fullNameTextBox.Visible = false;

                var loginLabel = new Label { Text = "Логин:", Location = new Point(20, 60), Size = new Size(80, 20) };
                var passwordLabel = new Label { Text = "Пароль:", Location = new Point(20, 90), Size = new Size(80, 20) };
                var fullNameLabel = new Label { Text = "ФИО:", Location = new Point(20, 120), Size = new Size(80, 20), Visible = false };
                var loginButton = new Button { Text = "Войти", Location = new Point(100, 150), Size = new Size(100, 25) };
                var registerButton = new Button { Text = "Регистрация", Location = new Point(210, 150), Size = new Size(100, 25) };

                loginButton.Click += (s, e) =>
                {
                    if (_isRegisterMode)
                        HandleCreate(_loginTextBox.Text, _passwordTextBox.Text, _fullNameTextBox.Text, fullNameLabel, _fullNameTextBox, loginButton, registerButton);
                    else
                        HandleLogin(_loginTextBox.Text, _passwordTextBox.Text);
                };
                registerButton.Click += (s, e) =>
                {
                    if (_isRegisterMode)
                        HandleBack(fullNameLabel, _fullNameTextBox, loginButton, registerButton);
                    else
                        HandleSwitchToRegister(fullNameLabel, _fullNameTextBox, loginButton, registerButton);
                };

                Controls.AddRange(new Control[] { titleLabel, loginLabel, _loginTextBox, passwordLabel, _passwordTextBox, fullNameLabel, _fullNameTextBox, loginButton, registerButton });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании UI: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void DrawTitleLabel(object? sender, PaintEventArgs e)
        {
            if (sender is not Label label) return;

            try
            {
                var graphics = e.Graphics;
                graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                var font = label.Font;

                TextRenderer.DrawText(graphics, "ТипаГос", font, new Rectangle(0, 0, label.Width, label.Height), Color.Blue, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
                var blueWidth = TextRenderer.MeasureText("ТипаГос", font).Width - 5;
                TextRenderer.DrawText(graphics, "Услуги", font, new Rectangle(blueWidth, 0, label.Width - blueWidth, label.Height), Color.Red, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка отрисовки заголовка: {ex.Message}");
            }
        }

        private void HandleLogin(string login, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Логин и пароль не могут быть пустыми.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var user = _authController.Login(login, password);
                if (user != null)
                {
                    Form nextForm = user.Role switch
                    {
                        Role.ADMIN => new AdminForm(user.Id, _serviceProvider.GetService<AdminController>() ?? throw new InvalidOperationException("AdminController not found."), _serviceProvider),
                        Role.CIVIL_SERVANT => new CivilServantForm(user.Id,
                            _serviceProvider.GetService<CivilServantController>() ?? throw new InvalidOperationException("CivilServantController not found."),
                            _serviceProvider),
                        Role.CITIZEN => new UserForm(user.Id, _serviceProvider.GetService<UserController>() ?? throw new InvalidOperationException("UserController not found."), _serviceProvider),
                        _ => throw new InvalidOperationException("Неизвестная роль")
                    };
                    Hide();
                    nextForm.ShowDialog();
                    Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HandleSwitchToRegister(Label fullNameLabel, TextBox fullNameTextBox, Button loginButton, Button registerButton)
        {
            _isRegisterMode = true;
            fullNameLabel.Visible = true;
            fullNameTextBox.Visible = true;
            loginButton.Text = "Создать";
            registerButton.Text = "Назад";
        }

        private void HandleCreate(string login, string password, string fullName, Label fullNameLabel, TextBox fullNameTextBox, Button loginButton, Button registerButton)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(fullName))
                {
                    MessageBox.Show("Все поля должны быть заполнены.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var user = _authController.RegisterUser(login, password, fullName);
                MessageBox.Show("Пользователь зарегистрирован. Ожидайте активации.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetRegisterMode(fullNameLabel, fullNameTextBox, loginButton, registerButton);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HandleBack(Label fullNameLabel, TextBox fullNameTextBox, Button loginButton, Button registerButton)
        {
            ResetRegisterMode(fullNameLabel, fullNameTextBox, loginButton, registerButton);
        }

        private void ResetRegisterMode(Label fullNameLabel, TextBox fullNameTextBox, Button loginButton, Button registerButton)
        {
            _isRegisterMode = false;
            fullNameLabel.Visible = false;
            fullNameTextBox.Visible = false;
            fullNameTextBox.Text = "";
            _loginTextBox.Text = "";
            _passwordTextBox.Text = "";
            loginButton.Text = "Войти";
            registerButton.Text = "Регистрация";
        }
    }
}