using GosuslugiWinForms.Controllers;
using GosuslugiWinForms.Models;


namespace GosuslugiWinForms.UI
{
    public partial class AdminForm : Form
    {
        private readonly AdminController _adminController;
        private readonly IServiceProvider _serviceProvider;
        private readonly Guid _adminId; 
        private DataGridView gridServices;
        private DataGridView gridRules;
        private DataGridView gridAccounts;

        public AdminForm(Guid adminId, AdminController adminController, IServiceProvider serviceProvider)
        {
            _adminId = adminId;
            _adminController = adminController ?? throw new ArgumentNullException(nameof(adminController));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            InitializeUI();
            LoadServices();
            LoadRules();
            LoadAccounts();
        }

        private void InitializeUI()
        {
            try
            {
                Text = "ТипаГосУслуги - Панель администратора";
                Size = new Size(800, 700);
                FormBorderStyle = FormBorderStyle.FixedSingle;
                MaximizeBox = false;

                var titleLabel = new Label
                {
                    Text = "",
                    Font = new Font("Arial", 16, FontStyle.Bold),
                    AutoSize = false,
                    Size = new Size(400, 40),
                    Location = new Point(20, 10)
                };
                titleLabel.Paint += (s, e) =>
                {
                    if (s is not Label label) return;
                    var graphics = e.Graphics;
                    graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                    var font = label.Font;
                    TextRenderer.DrawText(graphics, "ТипаГос", font, new Rectangle(0, 0, label.Width, label.Height), Color.Blue, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
                    var blueWidth = TextRenderer.MeasureText("ТипаГос", font).Width - 5;
                    TextRenderer.DrawText(graphics, "Услуги", font, new Rectangle(blueWidth, 0, label.Width - blueWidth, label.Height), Color.Red, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
                };

                var tabControl = new TabControl
                {
                    Location = new Point(20, 60),
                    Size = new Size(740, 480)
                };

                var tabServices = new TabPage { Text = "Услуги" };
                gridServices = new DataGridView
                {
                    Location = new Point(10, 10),
                    Size = new Size(700, 300),
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    ReadOnly = true,
                    Font = new Font("Arial Unicode MS", 10),
                    RowTemplate = { Height = 60 },
                    DefaultCellStyle = { WrapMode = DataGridViewTriState.True }
                };
                var btnAddService = new Button
                {
                    Text = "Добавить услугу",
                    Location = new Point(10, 320),
                    Size = new Size(120, 25)
                };
                btnAddService.Click += (s, e) =>
                {
                    try
                    {
                        using var editForm = new EditServiceForm(_adminController, _adminId);
                        if (editForm.ShowDialog() == DialogResult.OK)
                        {
                            LoadServices();
                            MessageBox.Show("Услуга добавлена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка добавления услуги: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                var btnEditService = new Button
                {
                    Text = "Редактировать услугу",
                    Location = new Point(140, 320),
                    Size = new Size(120, 25)
                };
               btnEditService.Click += (s, e) =>
                {
                    try
                    {
                        if (gridServices.SelectedRows.Count == 0)
                        {
                            MessageBox.Show("Выберите услугу.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        var selectedRow = gridServices.SelectedRows[0];
                        var serviceId = (Guid)selectedRow.Cells["Id"].Value;
                        var oldService = _adminController.GetService(serviceId);

                        var serviceCopy = new Service
                        {
                            Id = oldService.Id,
                            Title = oldService.Title,
                            Description = oldService.Description,
                            StartDate = oldService.StartDate,
                            ModifiedById = oldService.ModifiedById,
                            Rules = oldService.Rules != null ? oldService.Rules.Select(r => new Rule
                            {
                                Id = r.Id,
                                ServiceId = r.ServiceId,
                                ParameterTypeId = r.ParameterTypeId,
                                Description = r.Description,
                                Value = r.Value,
                                Operator = r.Operator,
                                DeadlineInterval = r.DeadlineInterval
                            }).ToList() : null
                        };
                        using var editForm = new EditServiceForm(_adminController, _adminId, serviceCopy);
                        if (editForm.ShowDialog() == DialogResult.OK)
                        {
                            var updatedOldService = new Service
                            {
                                Id = oldService.Id,
                                Title = oldService.Title,
                                Description = oldService.Description,
                                StartDate = oldService.StartDate,
                                EndDate = DateTimeOffset.UtcNow.UtcDateTime,
                                ModifiedById = oldService.ModifiedById
                            };
                            _adminController.UpdateService(serviceId, updatedOldService);

                            var newService = new Service
                            {
                                Id = Guid.NewGuid(),
                                Title = serviceCopy.Title,
                                Description = serviceCopy.Description,
                                StartDate = DateTimeOffset.UtcNow.UtcDateTime,
                                EndDate = null, 
                                ModifiedById = _adminId
                            };
                            _adminController.CreateService(newService);
                            // Копируем правила
                            if (serviceCopy.Rules != null)
                            {
                                foreach (var rule in serviceCopy.Rules)
                                {
                                    var newRule = new Rule
                                    {
                                        Id = Guid.NewGuid(),
                                        ServiceId = newService.Id,
                                        ParameterTypeId = rule.ParameterTypeId,
                                        Description = rule.Description,
                                        Value = rule.Value,
                                        Operator = rule.Operator,
                                        DeadlineInterval = rule.DeadlineInterval
                                    };
                                    _adminController.CreateRule(newRule);
                                }
                            }
                            LoadServices();
                            MessageBox.Show("Услуга обновлена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        var message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                        MessageBox.Show($"Ошибка редактирования услуги: {message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                tabServices.Controls.AddRange(new Control[] { gridServices, btnAddService, btnEditService });
                // Вкладка "Правила"
                var tabRules = new TabPage { Text = "Правила" };
                gridRules = new DataGridView
                {
                    Location = new Point(10, 10),
                    Size = new Size(700, 300),
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    ReadOnly = true,
                    Font = new Font("Arial Unicode MS", 10),
                    RowTemplate = { Height = 60 },
                    DefaultCellStyle = { WrapMode = DataGridViewTriState.True } 
                };
                var btnAddRule = new Button
                {
                    Text = "Добавить правило",
                    Location = new Point(10, 320),
                    Size = new Size(120, 25)
                };
                btnAddRule.Click += (s, e) =>
                {
                    try
                    {
                        using var editForm = new EditRuleForm(_adminController, _adminId);
                        if (editForm.ShowDialog() == DialogResult.OK)
                        {
                            LoadRules();
                            MessageBox.Show("Правило добавлено.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка добавления правила: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                var btnEditRule = new Button
                {
                    Text = "Редактировать правило",
                    Location = new Point(140, 320),
                    Size = new Size(120, 25)
                };
                btnEditRule.Click += (s, e) =>
                {
                    try
                    {
                        if (gridRules.SelectedRows.Count == 0)
                        {
                            MessageBox.Show("Выберите правило.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        var selectedRow = gridRules.SelectedRows[0];
                        var ruleId = (Guid)selectedRow.Cells["Id"].Value;
                        var rule = _adminController.GetRule(ruleId);
                        using var editForm = new EditRuleForm(_adminController, _adminId, rule);
                        if (editForm.ShowDialog() == DialogResult.OK)
                        {
                            LoadRules();
                            MessageBox.Show("Правило обновлено.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка редактирования правила: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                tabRules.Controls.AddRange(new Control[] { gridRules, btnAddRule, btnEditRule });

                // Вкладка "Учётные записи"
                var tabAccounts = new TabPage { Text = "Учётные записи" };
                gridAccounts = new DataGridView
                {
                    Location = new Point(10, 10),
                    Size = new Size(700, 250), 
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    ReadOnly = true,
                    Font = new Font("Arial Unicode MS", 10)
                };
                var lblFullName = new Label { Text = "ФИО:", Location = new Point(10, 270), Size = new Size(80, 20) };
                var txtFullName = new TextBox { Location = new Point(90, 270), Size = new Size(200, 20) };
                var lblLogin = new Label { Text = "Логин:", Location = new Point(10, 300), Size = new Size(80, 20) };
                var txtLogin = new TextBox { Location = new Point(90, 300), Size = new Size(200, 20) };
                var lblPassword = new Label { Text = "Пароль:", Location = new Point(10, 330), Size = new Size(80, 20) };
                var txtPassword = new TextBox { Location = new Point(90, 330), Size = new Size(200, 20), UseSystemPasswordChar = true };
                var lblRole = new Label { Text = "Роль:", Location = new Point(10, 360), Size = new Size(80, 20) };
                var cmbRole = new ComboBox
                {
                    Location = new Point(90, 360),
                    Size = new Size(200, 20),
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Font = new Font("Arial Unicode MS", 10)
                };
                cmbRole.Items.AddRange(new object[] { Role.ADMIN, Role.CIVIL_SERVANT });
                cmbRole.SelectedIndex = 0;
                var btnCreateAccount = new Button
                {
                    Text = "Создать учётную запись",
                    Location = new Point(90, 390),
                    Size = new Size(150, 25)
                };
                btnCreateAccount.Click += (s, e) =>
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(txtFullName.Text) || string.IsNullOrWhiteSpace(txtLogin.Text) || string.IsNullOrWhiteSpace(txtPassword.Text) || cmbRole.SelectedItem == null)
                        {
                            MessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        var account = new Account
                        {
                            Id = Guid.NewGuid(),
                            FullName = txtFullName.Text,
                            Login = txtLogin.Text,
                            Password = txtPassword.Text, 
                            Role = (Role)cmbRole.SelectedItem
                        };
                        _adminController.CreateStaffAccount(account);
                        LoadAccounts();
                        txtFullName.Text = txtLogin.Text = txtPassword.Text = "";
                        MessageBox.Show("Учётная запись создана.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка создания учётной записи: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                tabAccounts.Controls.AddRange(new Control[] { gridAccounts, lblFullName, txtFullName, lblLogin, txtLogin, lblPassword, txtPassword, lblRole, cmbRole, btnCreateAccount });

                tabControl.TabPages.AddRange(new[] { tabServices, tabRules, tabAccounts });

                var btnLogout = new Button
                {
                    Text = "Выйти",
                    Location = new Point(660, 10),
                    Size = new Size(100, 25),
                    BackColor = Color.White
                };
                btnLogout.Click += (s, e) =>
                {
                    var loginForm = new LoginForm();
                    loginForm.Show();
                    Close();
                };
                btnLogout.BringToFront();

                Controls.AddRange(new Control[] { titleLabel, tabControl, btnLogout });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании UI: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void LoadServices()
        {
            try
            {
                var services = _adminController.GetAllServices();
                gridServices.DataSource = services.Select(s => new
                {
                    s.Id,
                    s.Title,
                    s.Description,
                    s.StartDate,
                    s.EndDate
                }).ToList();
                if (gridServices.Columns["Id"] != null) gridServices.Columns["Id"].HeaderText = "ID";
                if (gridServices.Columns["Title"] != null) gridServices.Columns["Title"].HeaderText = "Название";
                if (gridServices.Columns["Description"] != null) gridServices.Columns["Description"].HeaderText = "Описание";
                if (gridServices.Columns["StartDate"] != null) gridServices.Columns["StartDate"].HeaderText = "Дата начала";
                if (gridServices.Columns["EndDate"] != null) gridServices.Columns["EndDate"].HeaderText = "Дата окончания";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки услуг: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadRules()
        {
            try
            {
                var rules = _adminController.GetAllRules();
                gridRules.DataSource = rules.Select(r => new
                {
                    r.Id,
                    r.ServiceId,
                    ParameterTypeName = r.ParameterType?.Name ?? "Неизвестно",
                    r.Description,
                    r.Value,
                    r.Operator,
                    r.DeadlineInterval
                }).ToList();
                if (gridRules.Columns["Id"] != null) gridRules.Columns["Id"].HeaderText = "ID";
                if (gridRules.Columns["ServiceId"] != null) gridRules.Columns["ServiceId"].HeaderText = "ID услуги";
                if (gridRules.Columns["ParameterTypeName"] != null) gridRules.Columns["ParameterTypeName"].HeaderText = "Тип параметра";
                if (gridRules.Columns["Description"] != null) gridRules.Columns["Description"].HeaderText = "Описание";
                if (gridRules.Columns["Value"] != null) gridRules.Columns["Value"].HeaderText = "Значение";
                if (gridRules.Columns["Operator"] != null) gridRules.Columns["Operator"].HeaderText = "Оператор";
                if (gridRules.Columns["DeadlineInterval"] != null) gridRules.Columns["DeadlineInterval"].HeaderText = "Срок";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки правил: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAccounts()
        {
            try
            {
                var accounts = _adminController.GetAllAccounts();
                gridAccounts.DataSource = accounts.Select(a => new
                {
                    a.Id,
                    a.FullName,
                    a.Login,
                    a.Role
                }).ToList();
                if (gridAccounts.Columns["Id"] != null) gridAccounts.Columns["Id"].HeaderText = "ID";
                if (gridAccounts.Columns["FullName"] != null) gridAccounts.Columns["FullName"].HeaderText = "ФИО";
                if (gridAccounts.Columns["Login"] != null) gridAccounts.Columns["Login"].HeaderText = "Логин";
                if (gridAccounts.Columns["Role"] != null) gridAccounts.Columns["Role"].HeaderText = "Роль";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки учётных записей: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}