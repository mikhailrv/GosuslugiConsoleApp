using GosuslugiWinForms.Controllers;
using GosuslugiWinForms.Models;

namespace GosuslugiWinForms.UI
{
    public class EditRuleForm : Form
    {
        private readonly AdminController _adminController;
        private readonly Guid _adminId;
        private readonly Rule? _rule;
        private ComboBox cmbService;
        private ComboBox cmbParameterType;
        private TextBox txtDescription;
        private TextBox txtValue;
        private ComboBox cmbOperator;
        private NumericUpDown nudDeadlineDays;

        public EditRuleForm(AdminController adminController, Guid adminId, Rule? rule = null)
        {
            _adminController = adminController ?? throw new ArgumentNullException(nameof(adminController));
            _adminId = adminId;
            _rule = rule;
            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            Text = _rule == null ? "Добавить правило" : "Редактировать правило";
            Size = new Size(400, 400);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            var lblService = new Label { Text = "Услуга:", Location = new Point(20, 20), Size = new Size(80, 20) };
            cmbService = new ComboBox
            {
                Location = new Point(100, 20),
                Size = new Size(260, 20),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            var lblParameterType = new Label { Text = "Тип параметра:", Location = new Point(20, 50), Size = new Size(80, 20) };
            cmbParameterType = new ComboBox
            {
                Location = new Point(100, 50),
                Size = new Size(260, 20),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            var lblDescription = new Label { Text = "Описание:", Location = new Point(20, 80), Size = new Size(80, 20) };
            txtDescription = new TextBox
            {
                Location = new Point(100, 80),
                Size = new Size(260, 100),
                Multiline = true,
                Text = _rule?.Description ?? ""
            };
            var lblValue = new Label { Text = "Значение:", Location = new Point(20, 190), Size = new Size(80, 20) };
            txtValue = new TextBox
            {
                Location = new Point(100, 190),
                Size = new Size(260, 20),
                Text = _rule?.Value ?? ""
            };
            var lblOperator = new Label { Text = "Оператор:", Location = new Point(20, 220), Size = new Size(80, 20) };
            cmbOperator = new ComboBox
            {
                Location = new Point(100, 220),
                Size = new Size(260, 20),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbOperator.Items.AddRange(Enum.GetValues(typeof(Operator)).Cast<object>().ToArray());
            var lblDeadline = new Label { Text = "Срок (дни):", Location = new Point(20, 250), Size = new Size(80, 20) };
            nudDeadlineDays = new NumericUpDown
            {
                Location = new Point(100, 250),
                Size = new Size(260, 20),
                Minimum = 1,
                Maximum = 365,
                Value = _rule?.DeadlineInterval.Days ?? 1
            };
            var btnSave = new Button
            {
                Text = "Сохранить",
                Location = new Point(100, 280),
                Size = new Size(100, 25)
            };
            btnSave.Click += (s, e) =>
            {
                try
                {
                    if (cmbService.SelectedItem == null || cmbParameterType.SelectedItem == null || string.IsNullOrWhiteSpace(txtDescription.Text) || cmbOperator.SelectedItem == null)
                    {
                        MessageBox.Show("Заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    var rule = new Rule
                    {
                        Id = _rule?.Id ?? Guid.NewGuid(), 
                        ServiceId = ((Service)cmbService.SelectedItem).Id,
                        ParameterTypeId = ((ParameterType)cmbParameterType.SelectedItem).Id,
                        Description = txtDescription.Text,
                        Value = txtValue.Text,
                        Operator = (Operator)cmbOperator.SelectedItem,
                        DeadlineInterval = TimeSpan.FromDays((int)nudDeadlineDays.Value)
                    };
                    if (_rule == null)
                    {
                        _adminController.CreateRule(rule);
                    }
                    else
                    {
                        _adminController.EditRule(_rule.Id, rule);
                    }
                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch (Exception ex)
                {
                    var message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    MessageBox.Show($"Ошибка сохранения правила: {message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            var btnCancel = new Button
            {
                Text = "Отмена",
                Location = new Point(210, 280),
                Size = new Size(100, 25)
            };
            btnCancel.Click += (s, e) => Close();

            Controls.AddRange(new Control[] { lblService, cmbService, lblParameterType, cmbParameterType, lblDescription, txtDescription, lblValue, txtValue, lblOperator, cmbOperator, lblDeadline, nudDeadlineDays, btnSave, btnCancel });
        }

        private void LoadData()
        {
            try
            {
                var services = _adminController.GetAllServices();
                cmbService.DataSource = services;
                cmbService.DisplayMember = "Title";
                cmbService.ValueMember = "Id";
                if (_rule != null && services.Any(s => s.Id == _rule.ServiceId))
                    cmbService.SelectedValue = _rule.ServiceId;

                var parameterTypes = _adminController.GetParameterTypes();
                cmbParameterType.DataSource = parameterTypes;
                cmbParameterType.DisplayMember = "Name";
                cmbParameterType.ValueMember = "Id";
                if (_rule != null && parameterTypes.Any(pt => pt.Id == _rule.ParameterTypeId))
                    cmbParameterType.SelectedValue = _rule.ParameterTypeId;

                if (_rule != null)
                    cmbOperator.SelectedItem = _rule.Operator;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}