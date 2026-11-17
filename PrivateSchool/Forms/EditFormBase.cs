using System;
using System.Windows.Forms;
using PrivateSchool.Models;
using PrivateSchool.Services;
using PrivateSchool.Utils;

namespace PrivateSchool.Forms
{
    /// <summary>
    /// Базова форма редагування (візуальне успадкування): кольорова шапка з заголовком,
    /// кнопки «Зберегти»/«Скасувати», єдине оформлення полів і обробка помилок валідації.
    /// Нащадки перевизначають метод SaveEntity().
    /// </summary>
    public partial class EditFormBase : Form
    {
        private readonly string _title;

        public EditFormBase()
        {
            InitializeComponent();
            AppTheme.StyleButton(btnSave, ButtonKind.Primary);
            AppTheme.StyleButton(btnCancel, ButtonKind.Secondary);
        }

        protected EditFormBase(SchoolService service, bool isNew, string newTitle, string editTitle) : this()
        {
            Service = service;
            IsNew = isNew;
            _title = isNew ? newTitle : editTitle;
        }

        protected SchoolService Service { get; private set; }

        /// <summary>true — створюється новий запис, false — редагується існуючий.</summary>
        protected bool IsNew { get; private set; }

        /// <summary>Підзаголовок у шапці форми (наприклад, ПІБ учня, що редагується).</summary>
        protected string Subtitle
        {
            get { return lblHeaderSubtitle.Text; }
            set { lblHeaderSubtitle.Text = value; }
        }

        /// <summary>Зчитує дані з елементів керування та зберігає запис через сервіс.
        /// У разі помилки має згенерувати ValidationException.</summary>
        protected virtual void SaveEntity()
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Заголовок задається тут, бо InitializeComponent() нащадка перезаписує Text
            if (!string.IsNullOrEmpty(_title))
            {
                Text = _title;
                lblHeaderTitle.Text = _title;
            }
            AppTheme.ApplyToInputs(this);
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
