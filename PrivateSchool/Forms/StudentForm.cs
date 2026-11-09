using System;
using System.Linq;
using PrivateSchool.Models;
using PrivateSchool.Services;
using PrivateSchool.Utils;

namespace PrivateSchool.Forms
{
    /// <summary>
    /// Форма додавання / редагування учня.
    /// </summary>
    public partial class StudentForm : EditFormBase
    {
        private readonly Student _student;

        public StudentForm(SchoolService service, Student student)
            : base(service, student == null, "учень")
        {
            InitializeComponent();
            _student = student == null ? new Student() : student.Clone<Student>();
            if (student == null)
                _student.ContractNumber = SuggestContractNumber();
            LoadData();

            cmbGroup.SelectedIndexChanged += (s, e) => UpdateFeeInfo();
            numDiscount.ValueChanged += (s, e) => UpdateFeeInfo();
            UpdateFeeInfo();
        }

        private string SuggestContractNumber()
        {
            int next = Service.Students.Count == 0 ? 1 : Service.Students.GetAll().Max(s => s.Id) + 1;
            return string.Format("Д-{0:yy}/{1:000}", DateTime.Today, next);
        }

        private void LoadData()
        {
            txtLastName.Text = _student.LastName;
            txtFirstName.Text = _student.FirstName;
            txtMiddleName.Text = _student.MiddleName;
            dtpBirthDate.Value = _student.BirthDate;
            txtPhone.Text = _student.Phone;
            txtEmail.Text = _student.Email;
            txtParentName.Text = _student.ParentName;
            txtParentPhone.Text = _student.ParentPhone;
            txtContract.Text = _student.ContractNumber;
            dtpEnrollment.Value = _student.EnrollmentDate;
            numDiscount.Value = _student.Discount;

            var groups = new[] { new ComboItem(0, "— не зараховано —") }
                .Concat(Service.Groups.GetAll().OrderBy(g => g.Name).Select(g =>
                    new ComboItem(g.Id, string.Format("{0} — {1} ({2}/{3})", g.Name, Service.GetCourseName(g.CourseId),
                        Service.GetActiveStudentCount(g.Id), g.MaxStudents))));
            UiHelper.Fill(cmbGroup, groups, _student.GroupId);
            UiHelper.Fill(cmbStatus, EnumHelper.GetItems<StudentStatus>(), _student.Status);
        }

        private void UpdateFeeInfo()
        {
            Course course = Service.GetCourseOfGroup(UiHelper.GetValue<int>(cmbGroup));
            if (course == null)
            {
                lblFee.Text = "Оплата: група не обрана";
                return;
            }
            Student probe = new Student { Discount = numDiscount.Value };
            lblFee.Text = string.Format("Оплата: {0:N2} грн/міс.", probe.CalculateMonthlyFee(course));
        }

        protected override void SaveEntity()
        {
            _student.LastName = txtLastName.Text;
            _student.FirstName = txtFirstName.Text;
            _student.MiddleName = txtMiddleName.Text;
            _student.BirthDate = dtpBirthDate.Value.Date;
            _student.Phone = txtPhone.Text;
            _student.Email = txtEmail.Text;
            _student.ParentName = txtParentName.Text;
            _student.ParentPhone = txtParentPhone.Text;
            _student.ContractNumber = txtContract.Text;
            _student.EnrollmentDate = dtpEnrollment.Value.Date;
            _student.Discount = numDiscount.Value;
            _student.GroupId = UiHelper.GetValue<int>(cmbGroup);
            _student.Status = UiHelper.GetValue<StudentStatus>(cmbStatus);

            if (IsNew)
                Service.AddStudent(_student);
            else
                Service.UpdateStudent(_student);
        }
    }
}
