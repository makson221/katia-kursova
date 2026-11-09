using PrivateSchool.Models;
using PrivateSchool.Services;
using PrivateSchool.Utils;

namespace PrivateSchool.Forms
{
    /// <summary>
    /// Форма додавання / редагування навчального курсу.
    /// </summary>
    public partial class CourseForm : EditFormBase
    {
        private readonly Course _course;

        public CourseForm(SchoolService service, Course course)
            : base(service, course == null, "курс")
        {
            InitializeComponent();
            _course = course == null ? new Course { PricePerMonth = 2000 } : course.Clone<Course>();
            LoadData();

            numPrice.ValueChanged += (s, e) => UpdateTotal();
            numDuration.ValueChanged += (s, e) => UpdateTotal();
            UpdateTotal();
        }

        private void LoadData()
        {
            txtName.Text = _course.Name;
            txtDescription.Text = _course.Description;
            numPrice.Value = _course.PricePerMonth;
            numDuration.Value = _course.DurationMonths;
            numLessonsPerWeek.Value = _course.LessonsPerWeek;
            UiHelper.Fill(cmbLevel, EnumHelper.GetItems<CourseLevel>(), _course.Level);
        }

        private void UpdateTotal()
        {
            lblTotal.Text = string.Format("Повна вартість курсу: {0:N2} грн", numPrice.Value * numDuration.Value);
        }

        protected override void SaveEntity()
        {
            _course.Name = txtName.Text;
            _course.Description = txtDescription.Text;
            _course.PricePerMonth = numPrice.Value;
            _course.DurationMonths = (int)numDuration.Value;
            _course.LessonsPerWeek = (int)numLessonsPerWeek.Value;
            _course.Level = UiHelper.GetValue<CourseLevel>(cmbLevel);

            if (IsNew)
                Service.AddCourse(_course);
            else
                Service.UpdateCourse(_course);
        }
    }
}
