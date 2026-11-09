using System;
using System.Windows.Forms;
using PrivateSchool.Models;
using PrivateSchool.Services;

namespace PrivateSchool.Forms
{
    /// <summary>
    /// Базова форма редагування (візуальне успадкування): містить кнопки
    /// «Зберегти»/«Скасувати» і єдину обробку помилок валідації.
    /// Нащадки перевизначають метод SaveEntity().
    /// </summary>
    public partial class EditFormBase : Form
    {
        public EditFormBase()
        {
            InitializeComponent();
        }

        protected EditFormBase(SchoolService service, bool isNew, string entityName) : this()
        {
            Service = service;
            IsNew = isNew;
            _entityName = entityName;
        }

        private readonly string _entityName;

        protected SchoolService Service { get; private set; }

        /// <summary>true — створюється новий запис, false — редагується існуючий.</summary>
        protected bool IsNew { get; private set; }

        /// <summary>Зчитує дані з елементів керування та зберігає запис через сервіс.
        /// У разі помилки має згенерувати ValidationException.</summary>
        protected virtual void SaveEntity()
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Заголовок задається тут, бо InitializeComponent() нащадка перезаписує Text
            if (!string.IsNullOrEmpty(_entityName))
                Text = (IsNew ? "Додавання: " : "Редагування: ") + _entityName;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveEntity();
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (ValidationException ex)
            {
                MessageBox.Show(this, ex.Message, "Перевірка даних", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
