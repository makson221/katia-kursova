using System;
using PrivateSchool.Models;
using PrivateSchool.Services;
using PrivateSchool.Utils;

namespace PrivateSchool.Forms
{
    /// <summary>
    /// Форма додавання / редагування викладача.
    /// </summary>
    public partial class TeacherForm : EditFormBase
    {
        private readonly Teacher _teacher;

        public TeacherForm(SchoolService service, Teacher teacher)
            : base(service, teacher == null, "викладач")
        {
            InitializeComponent();
            _teacher = teacher == null ? new Teacher { BirthDate = DateTime.Today.AddYears(-30) } : teacher.Clone<Teacher>();
            LoadData();
        }

        private void LoadData()
        {
            txtLastName.Text = _teacher.LastName;
            txtFirstName.Text = _teacher.FirstName;
            txtMiddleName.Text = _teacher.MiddleName;
            dtpBirthDate.Value = _teacher.BirthDate;
            txtPhone.Text = _teacher.Phone;
            txtEmail.Text = _teacher.Email;
            txtSpecialization.Text = _teacher.Specialization;
            txtEducation.Text = _teacher.Education;
            numExperience.Value = _teacher.ExperienceYears;
            numHourlyRate.Value = _teacher.HourlyRate;
            dtpHireDate.Value = _teacher.HireDate;
            UiHelper.Fill(cmbCategory, EnumHelper.GetItems<TeacherCategory>(), _teacher.Category);
        }

        protected override void SaveEntity()
        {
            _teacher.LastName = txtLastName.Text;
            _teacher.FirstName = txtFirstName.Text;
            _teacher.MiddleName = txtMiddleName.Text;
            _teacher.BirthDate = dtpBirthDate.Value.Date;
            _teacher.Phone = txtPhone.Text;
            _teacher.Email = txtEmail.Text;
            _teacher.Specialization = txtSpecialization.Text;
            _teacher.Education = txtEducation.Text;
            _teacher.ExperienceYears = (int)numExperience.Value;
            _teacher.HourlyRate = numHourlyRate.Value;
            _teacher.HireDate = dtpHireDate.Value.Date;
            _teacher.Category = UiHelper.GetValue<TeacherCategory>(cmbCategory);

            if (IsNew)
                Service.AddTeacher(_teacher);
            else
                Service.UpdateTeacher(_teacher);
        }
    }
}
