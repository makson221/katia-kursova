using System;
using System.Linq;
using PrivateSchool.Models;
using PrivateSchool.Services;
using PrivateSchool.Utils;

namespace PrivateSchool.Forms
{
    /// <summary>
    /// Форма реєстрації / редагування оплати за навчання.
    /// </summary>
    public partial class PaymentForm : EditFormBase
    {
        private readonly Payment _payment;

        public PaymentForm(SchoolService service, Payment payment, int defaultStudentId)
            : base(service, payment == null, "оплата")
        {
            InitializeComponent();
            _payment = payment == null ? new Payment { StudentId = defaultStudentId } : payment.Clone<Payment>();
            dtpDate.MaxDate = DateTime.Today;
            LoadData();

            cmbStudent.SelectedIndexChanged += (s, e) => OnStudentChanged();
            OnStudentChanged();
        }

        private void LoadData()
        {
            UiHelper.Fill(cmbStudent, Service.Students.GetAll().OrderBy(s => s.FullName)
                .Select(s => new ComboItem(s.Id, s.FullName + " (" + Service.GetGroupName(s.GroupId) + ")")),
                _payment.StudentId);
            UiHelper.Fill(cmbMethod, EnumHelper.GetItems<PaymentMethod>(), _payment.Method);
            dtpDate.Value = _payment.Date > DateTime.Today ? DateTime.Today : _payment.Date;
            numAmount.Value = _payment.Amount;
            txtComment.Text = _payment.Comment;
        }

        private void OnStudentChanged()
        {
            Student student = Service.Students.GetById(UiHelper.GetValue<int>(cmbStudent));
            if (student == null)
            {
                lblInfo.Text = string.Empty;
                return;
            }

            decimal fee = Service.GetMonthlyFee(student);
            decimal debt = Service.GetDebt(student);
            lblInfo.Text = string.Format("Щомісячна оплата: {0:N2} грн\n{1}: {2:N2} грн",
                fee, debt >= 0 ? "Заборгованість" : "Переплата", Math.Abs(debt));

            // Для нової оплати пропонуємо суму боргу або місячної оплати
            if (IsNew)
            {
                decimal suggested = debt > 0 ? debt : fee;
                numAmount.Value = Math.Min(numAmount.Maximum, suggested);
                if (txtComment.Text.Length == 0 || txtComment.Text.StartsWith("Оплата за "))
                    txtComment.Text = "Оплата за " + DateTime.Today.ToString("MMMM yyyy");
            }
        }

        protected override void SaveEntity()
        {
            _payment.StudentId = UiHelper.GetValue<int>(cmbStudent);
            _payment.Date = dtpDate.Value.Date;
            _payment.Amount = numAmount.Value;
            _payment.Method = UiHelper.GetValue<PaymentMethod>(cmbMethod);
            _payment.Comment = txtComment.Text;

            if (IsNew)
                Service.AddPayment(_payment);
            else
                Service.UpdatePayment(_payment);
        }
    }
}
