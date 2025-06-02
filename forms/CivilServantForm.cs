using GosuslugiWinForms.Controllers;
using GosuslugiWinForms.Models;


namespace GosuslugiWinForms.UI
{
    public class CivilServantForm : Form
    {
        private readonly CivilServantController _controller;
        private readonly IServiceProvider _serviceProvider;
        private readonly Guid _civilServantId;
        private DataGridView gridApplications;
        private DataGridView gridRegistrations;

        public CivilServantForm(Guid civilServantId, CivilServantController controller, IServiceProvider serviceProvider)
        {
            _civilServantId = civilServantId;
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            InitializeUI();
            LoadApplications();
            LoadRegistrations();
        }

        private void InitializeUI()
        {
            try
            {
                Text = "ТипаГосУслуги - Панель госслужащего";
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
                    Size = new Size(740, 580)
                };

                // Вкладка "Заявки"
                var tabApplications = new TabPage { Text = "Заявки" };
                gridApplications = new DataGridView
                {
                    Location = new Point(10, 10),
                    Size = new Size(700, 300),
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    ReadOnly = true,
                    Font = new Font("Arial Unicode MS", 10)
                };
                var lblStatus = new Label { Text = "Статус:", Location = new Point(10, 320), Size = new Size(80, 20) };
                var cmbStatus = new ComboBox
                {
                    Location = new Point(90, 320),
                    Size = new Size(200, 20),
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                cmbStatus.Items.AddRange(Enum.GetValues(typeof(ApplicationStatus)).Cast<object>().ToArray());
                var lblResult = new Label { Text = "Результат:", Location = new Point(10, 350), Size = new Size(80, 20) };
                var txtResult = new TextBox
                {
                    Location = new Point(90, 350),
                    Size = new Size(200, 100),
                    Multiline = true
                };
                var btnUpdateApplication = new Button
                {
                    Text = "Обновить заявку",
                    Location = new Point(90, 460),
                    Size = new Size(120, 25)
                };
                btnUpdateApplication.Click += (s, e) =>
                {
                    try
                    {
                        if (gridApplications.SelectedRows.Count == 0)
                        {
                            MessageBox.Show("Выберите заявку.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (cmbStatus.SelectedItem == null)
                        {
                            MessageBox.Show("Выберите статус.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        var selectedRow = gridApplications.SelectedRows[0];
                        var applicationId = (Guid)selectedRow.Cells["Id"].Value;
                        var status = (ApplicationStatus)cmbStatus.SelectedItem;
                        var result = txtResult.Text;
                        _controller.UpdateApplication(applicationId, status, result);
                        LoadApplications();
                        txtResult.Text = "";
                        MessageBox.Show("Заявка обновлена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка обновления заявки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                tabApplications.Controls.AddRange(new Control[] { gridApplications, lblStatus, cmbStatus, lblResult, txtResult, btnUpdateApplication });

                // Вкладка "Регистрации"
                var tabRegistrations = new TabPage { Text = "Регистрации" };
                gridRegistrations = new DataGridView
                {
                    Location = new Point(10, 10),
                    Size = new Size(700, 300),
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    ReadOnly = true,
                    Font = new Font("Arial Unicode MS", 10)
                };
                var btnApproveRegistration = new Button
                {
                    Text = "Активировать",
                    Location = new Point(10, 320),
                    Size = new Size(120, 25)
                };
                btnApproveRegistration.Click += (s, e) =>
                {
                    try
                    {
                        if (gridRegistrations.SelectedRows.Count == 0)
                        {
                            MessageBox.Show("Выберите регистрацию.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        var selectedRow = gridRegistrations.SelectedRows[0];
                        var accountId = (Guid)selectedRow.Cells["Id"].Value;
                        _controller.ProcessRegistrationRequest(accountId, true);
                        LoadRegistrations();
                        MessageBox.Show("Учётная запись активирована.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка активации: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                var btnRejectRegistration = new Button
                {
                    Text = "Отклонить",
                    Location = new Point(140, 320),
                    Size = new Size(120, 25)
                };
                btnRejectRegistration.Click += (s, e) =>
                {
                    try
                    {
                        if (gridRegistrations.SelectedRows.Count == 0)
                        {
                            MessageBox.Show("Выберите регистрацию.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        var selectedRow = gridRegistrations.SelectedRows[0];
                        var accountId = (Guid)selectedRow.Cells["Id"].Value;
                        _controller.ProcessRegistrationRequest(accountId, false);
                        LoadRegistrations();
                        MessageBox.Show("Учётная запись отклонена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка отклонения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                tabRegistrations.Controls.AddRange(new Control[] { gridRegistrations, btnApproveRegistration, btnRejectRegistration });

                tabControl.TabPages.AddRange(new[] { tabApplications, tabRegistrations });

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

        private void LoadApplications()
        {
            try
            {
                var applications = _controller.GetAllApplications();
                gridApplications.DataSource = applications.Select(a => new
                {
                    a.Id,
                    a.UserId,
                    ServiceTitle = a.Service?.Title ?? "Неизвестно",
                    a.Status,
                    a.Result,
                    a.SubmissionDate,
                    a.Deadline
                }).ToList();
                if (gridApplications.Columns["Id"] != null) gridApplications.Columns["Id"].HeaderText = "ID";
                if (gridApplications.Columns["UserId"] != null) gridApplications.Columns["UserId"].HeaderText = "ID пользователя";
                if (gridApplications.Columns["ServiceTitle"] != null) gridApplications.Columns["ServiceTitle"].HeaderText = "Услуга";
                if (gridApplications.Columns["Status"] != null) gridApplications.Columns["Status"].HeaderText = "Статус";
                if (gridApplications.Columns["Result"] != null) gridApplications.Columns["Result"].HeaderText = "Результат";
                if (gridApplications.Columns["SubmissionDate"] != null) gridApplications.Columns["SubmissionDate"].HeaderText = "Дата подачи";
                if (gridApplications.Columns["Deadline"] != null) gridApplications.Columns["Deadline"].HeaderText = "Срок";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки заявок: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LoadRegistrations()
        {
            try
            {
                var registrations = _controller.GetPendingRegistrations();
                gridRegistrations.DataSource = registrations.Select(r => new
                {
                    r.Id,
                    r.FullName,
                    r.Login,
                    r.Role
                }).ToList();
                if (gridRegistrations.Columns["Id"] != null) gridRegistrations.Columns["Id"].HeaderText = "ID";
                if (gridRegistrations.Columns["FullName"] != null) gridRegistrations.Columns["FullName"].HeaderText = "ФИО";
                if (gridRegistrations.Columns["Login"] != null) gridRegistrations.Columns["Login"].HeaderText = "Логин";
                if (gridRegistrations.Columns["Role"] != null) gridRegistrations.Columns["Role"].HeaderText = "Роль";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки регистраций: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}