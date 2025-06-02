using System;
using System.Drawing;
using System.Windows.Forms;
using GosuslugiWinForms.Controllers;
using GosuslugiWinForms.Infrastructure;
using GosuslugiWinForms.Models;
using Microsoft.Extensions.DependencyInjection;

namespace GosuslugiWinForms.UI
{
    public partial class UserForm : Form
    {
        private readonly UserController _userController;
        private readonly IServiceProvider _serviceProvider;
        private readonly Guid _userId;
        private TextBox txtFullName;
        private ComboBox cmbParameterType;
        private TextBox txtParameterValue;
        private DataGridView gridParameters;
        private DataGridView gridApplications;

        public UserForm(Guid userId, UserController userController, IServiceProvider serviceProvider)
        {
            _userId = userId;
            _userController = userController ?? throw new ArgumentNullException(nameof(userController));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            InitializeUI();
            LoadUserData();
            LoadParameters();
            LoadApplications();
        }

        private void InitializeUI()
        {
            try
            {
                Text = "ТипаГосУслуги - Личный кабинет";
                Size = new Size(700, 500);
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
                    Size = new Size(640, 380)
                };

                // Вкладка "Мои заявки"
                var tabApplications = new TabPage { Text = "Мои заявки" };
                gridApplications = new DataGridView
                {
                    Location = new Point(10, 10),
                    Size = new Size(600, 280),
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    ReadOnly = true,
                    Font = new Font("Arial Unicode MS", 10)
                };
                var btnCreateApplication = new Button
                {
                    Text = "Создать заявку",
                    Location = new Point(10, 300),
                    Size = new Size(120, 25)
                };
                btnCreateApplication.Click += (s, e) =>
                {
                    try
                    {
                        using var createForm = new CreateApplicationForm(_userId, _userController);
                        if (createForm.ShowDialog() == DialogResult.OK)
                        {
                            LoadApplications();
                            MessageBox.Show("Заявка создана.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка создания заявки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                var btnCancelApplication = new Button
                {
                    Text = "Отменить заявку",
                    Location = new Point(140, 300),
                    Size = new Size(120, 25)
                };
                btnCancelApplication.Click += (s, e) =>
                {
                    try
                    {
                        if (gridApplications.SelectedRows.Count == 0)
                        {
                            MessageBox.Show("Выберите заявку.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        var selectedRow = gridApplications.SelectedRows[0];
                        var applicationId = (Guid)selectedRow.Cells["Id"].Value;
                        _userController.CancelApplication(applicationId);
                        LoadApplications();
                        MessageBox.Show("Заявка отменена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка отмены заявки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                tabApplications.Controls.AddRange(new Control[] { gridApplications, btnCreateApplication, btnCancelApplication });

                // Вкладка "Мои данные"
                var tabProfile = new TabPage { Text = "Мои данные" };
                var lblFullName = new Label { Text = "ФИО:", Location = new Point(10, 10), Size = new Size(80, 20) };
                txtFullName = new TextBox { Location = new Point(90, 10), Size = new Size(200, 20) };
                var btnSaveProfile = new Button
                {
                    Text = "Сохранить ФИО",
                    Location = new Point(90, 40),
                    Size = new Size(120, 25)
                };
                btnSaveProfile.Click += (s, e) =>
                {
                    try
                    {
                        _userController.UpdateUser(_userId, txtFullName.Text);
                        MessageBox.Show("ФИО сохранено.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка сохранения ФИО: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                var lblParameters = new Label { Text = "Параметры:", Location = new Point(10, 80), Size = new Size(80, 20) };
                gridParameters = new DataGridView
                {
                    Location = new Point(10, 100),
                    Size = new Size(600, 150),
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    ReadOnly = true,
                    Font = new Font("Arial Unicode MS", 10) 
                };
                var lblParameterType = new Label { Text = "Тип параметра:", Location = new Point(10, 260), Size = new Size(80, 20) };
                cmbParameterType = new ComboBox
                {
                    Location = new Point(90, 260),
                    Size = new Size(200, 20),
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Font = new Font("Arial Unicode MS", 10) // Поддержка кириллицы
                };
                var lblParameterValue = new Label { Text = "Значение:", Location = new Point(10, 290), Size = new Size(80, 20) };
                txtParameterValue = new TextBox { Location = new Point(90, 290), Size = new Size(200, 20) };
                var btnAddParameter = new Button
                {
                    Text = "Добавить/изменить",
                    Location = new Point(90, 320),
                    Size = new Size(200, 25) 
                };
                btnAddParameter.Click += (s, e) =>
                {
                    try
                    {
                        if (cmbParameterType.SelectedItem == null || string.IsNullOrWhiteSpace(txtParameterValue.Text))
                        {
                            MessageBox.Show("Выберите тип параметра и введите значение.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        var parameterTypeId = ((ParameterType)cmbParameterType.SelectedItem).Id;
                        var existingParameter = _userController.GetUserParameters(_userId)
                            .FirstOrDefault(p => p.ParameterTypeId == parameterTypeId);
                        if (existingParameter != null)
                        {
                            _userController.EditParameter(_userId, existingParameter.Id, txtParameterValue.Text);
                            MessageBox.Show("Параметр обновлён.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            var parameter = new Parameter
                            {
                                Id = Guid.NewGuid(),
                                UserId = _userId,
                                ParameterTypeId = parameterTypeId,
                                Value = txtParameterValue.Text
                            };
                            _userController.AddParameter(parameter);
                            MessageBox.Show("Параметр добавлен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        LoadParameters();
                        txtParameterValue.Text = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка изменения параметра: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                tabProfile.Controls.AddRange(new Control[] { lblFullName, txtFullName, btnSaveProfile, lblParameters, gridParameters, lblParameterType, cmbParameterType, lblParameterValue, txtParameterValue, btnAddParameter });
                tabControl.TabPages.AddRange(new[] { tabApplications, tabProfile });

            var btnLogout = new Button
            {
                Text = "Выйти",
                Location = new Point(500, 10), 
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

        private void LoadUserData()
        {
            try
            {
                var user = _userController.GetUser(_userId);
                if (txtFullName != null)
                {
                    txtFullName.Text = user.FullName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadParameters()
        {
            try
            {
                var parameters = _userController.GetUserParameters(_userId);
                if (gridParameters != null && cmbParameterType != null)
                {
                    gridParameters.DataSource = parameters.Select(p => new
                    {
                        ParameterTypeName = p.ParameterType?.Name ?? "Неизвестно",
                        p.Value
                    }).ToList();
                    if (gridParameters.Columns["ParameterTypeName"] != null) gridParameters.Columns["ParameterTypeName"].HeaderText = "Тип параметра";
                    if (gridParameters.Columns["Value"] != null) gridParameters.Columns["Value"].HeaderText = "Значение";

                    var parameterTypes = _userController.GetParameterTypes();
                    cmbParameterType.DataSource = parameterTypes;
                    cmbParameterType.DisplayMember = "Name";
                    cmbParameterType.ValueMember = "Id";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки параметров: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadApplications()
        {
            try
            {
                var applications = _userController.GetApplicationsByUser(_userId);
                if (gridApplications != null)
                {
                    gridApplications.DataSource = applications.Select(a => new
                    {
                        a.Id, 
                        a.Status,
                        a.Result,
                        a.SubmissionDate,
                        a.Deadline
                    }).ToList();
                    if (gridApplications.Columns["Id"] != null) gridApplications.Columns["Id"].Visible = false; // Скрываем Id
                    if (gridApplications.Columns["Status"] != null) gridApplications.Columns["Status"].HeaderText = "Статус";
                    if (gridApplications.Columns["Result"] != null) gridApplications.Columns["Result"].HeaderText = "Результат";
                    if (gridApplications.Columns["SubmissionDate"] != null) gridApplications.Columns["SubmissionDate"].HeaderText = "Дата подачи";
                    if (gridApplications.Columns["Deadline"] != null) gridApplications.Columns["Deadline"].HeaderText = "Срок исполнения";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки заявок: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}