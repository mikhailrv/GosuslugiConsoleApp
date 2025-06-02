using GosuslugiWinForms.Controllers;
using GosuslugiWinForms.Models;


namespace GosuslugiWinForms.UI
{
    public partial class CreateApplicationForm : Form
    {
        private readonly UserController _userController;
        private readonly Guid _userId;
        private ComboBox cmbService; 
        public CreateApplicationForm(Guid userId, UserController userController)
        {
            _userId = userId;
            _userController = userController ?? throw new ArgumentNullException(nameof(userController));
            InitializeUI();
            LoadServices();
        }

        private void InitializeUI()
        {
            try
            {
                Text = "Создание заявки";
                Size = new Size(400, 200);
                FormBorderStyle = FormBorderStyle.FixedDialog;
                MaximizeBox = false;
                MinimizeBox = false;
                StartPosition = FormStartPosition.CenterParent;

                var lblService = new Label
                {
                    Text = "Услуга:",
                    Location = new Point(20, 20),
                    Size = new Size(80, 20)
                };
                cmbService = new ComboBox
                {
                    Location = new Point(100, 20),
                    Size = new Size(200, 20),
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                var btnSubmit = new Button
                {
                    Text = "Отправить",
                    Location = new Point(100, 100),
                    Size = new Size(100, 25)
                };
                btnSubmit.Click += (s, e) =>
                {
                    try
                    {
                        if (cmbService == null || cmbService.SelectedItem == null)
                        {
                            MessageBox.Show("Выберите услугу.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        var serviceId = ((Service)cmbService.SelectedItem).Id;
                        _userController.SubmitApplication(serviceId, _userId);
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    catch (InvalidOperationException ex) when (ex.Message == "Вы не можете получить эту услугу")
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };
                var btnCancel = new Button
                {
                    Text = "Отмена",
                    Location = new Point(210, 100),
                    Size = new Size(100, 25)
                };
                btnCancel.Click += (s, e) =>
                {
                    DialogResult = DialogResult.Cancel;
                    Close();
                };

                Controls.AddRange(new Control[] { lblService, cmbService, btnSubmit, btnCancel });
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
                var services = _userController.GetAllActiveServices();
                if (cmbService != null)
                {
                    cmbService.DataSource = services;
                    cmbService.DisplayMember = "Title";
                    cmbService.ValueMember = "Id";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки услуг: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}