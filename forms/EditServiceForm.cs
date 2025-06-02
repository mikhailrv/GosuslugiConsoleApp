using GosuslugiWinForms.Controllers;
using GosuslugiWinForms.Models;

namespace GosuslugiWinForms.UI
{
    public class EditServiceForm : Form
    {
        private readonly AdminController _adminController;
        private readonly Guid _adminId;
        private readonly Service? _service;
        private TextBox txtTitle;
        private TextBox txtDescription;
        private ListBox lstRules;

        public EditServiceForm(AdminController adminController, Guid adminId, Service? service = null)
        {
            _adminController = adminController ?? throw new ArgumentNullException(nameof(adminController));
            _adminId = adminId;
            _service = service;
            InitializeUI();
            LoadRules();
        }

        private void InitializeUI()
        {
            Text = _service == null ? "Добавить услугу" : "Редактировать услугу";
            Size = new Size(400, 400);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            var lblTitle = new Label { Text = "Название:", Location = new Point(20, 20), Size = new Size(80, 20) };
            txtTitle = new TextBox
            {
                Location = new Point(100, 20),
                Size = new Size(260, 20),
                Text = _service?.Title ?? ""
            };
            var lblDescription = new Label { Text = "Описание:", Location = new Point(20, 50), Size = new Size(80, 20) };
            txtDescription = new TextBox
            {
                Location = new Point(100, 50),
                Size = new Size(260, 100),
                Multiline = true,
                Text = _service?.Description ?? ""
            };
            var lblRules = new Label { Text = "Правила:", Location = new Point(20, 160), Size = new Size(80, 20) };
            lstRules = new ListBox
            {
                Location = new Point(100, 160),
                Size = new Size(260, 100),
                SelectionMode = SelectionMode.MultiExtended
            };
            var btnSave = new Button
            {
                Text = "Сохранить",
                Location = new Point(100, 270),
                Size = new Size(100, 25)
            };
            btnSave.Click += (s, e) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(txtTitle.Text))
                    {
                        MessageBox.Show("Введите название услуги.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    var service = new Service
                    {
                        Id = _service?.Id ?? Guid.NewGuid(),
                        Title = txtTitle.Text,
                        Description = txtDescription.Text,
                        StartDate = DateTimeOffset.UtcNow.UtcDateTime,
                        ModifiedById = _adminId
                    };
                    if (_service == null)
                    {
                        _adminController.CreateService(service);
                        // Создаём копии выбранных правил
                        foreach (Rule selectedRule in lstRules.SelectedItems)
                        {
                            var newRule = new Rule
                            {
                                Id = Guid.NewGuid(),
                                ServiceId = service.Id,
                                ParameterTypeId = selectedRule.ParameterTypeId,
                                Description = selectedRule.Description,
                                Value = selectedRule.Value,
                                Operator = selectedRule.Operator,
                                DeadlineInterval = selectedRule.DeadlineInterval
                            };
                            _adminController.CreateRule(newRule);
                        }
                    }
                    else
                    {
                        _adminController.UpdateService(_service.Id, service);
                    }
                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch (Exception ex)
                {
                    var message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    MessageBox.Show($"Ошибка сохранения услуги: {message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            var btnCancel = new Button
            {
                Text = "Отмена",
                Location = new Point(210, 270),
                Size = new Size(100, 25)
            };
            btnCancel.Click += (s, e) => Close();

            Controls.AddRange(new Control[] { lblTitle, txtTitle, lblDescription, txtDescription, lblRules, lstRules, btnSave, btnCancel });
        }

        private void LoadRules()
        {
            try
            {
                var rules = _adminController.GetAllRules();
                lstRules.DataSource = rules;
                lstRules.DisplayMember = "Description";
                lstRules.ValueMember = "Id";
                if (_service != null && _service.Rules != null)
                {
                    foreach (var rule in _service.Rules)
                    {
                        var index = rules.FindIndex(r => r.Id == rule.Id);
                        if (index >= 0)
                            lstRules.SetSelected(index, true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки правил: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}