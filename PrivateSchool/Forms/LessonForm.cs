using System;
using System.Linq;
using PrivateSchool.Models;
using PrivateSchool.Services;
using PrivateSchool.Utils;

namespace PrivateSchool.Forms
{
    /// <summary>
    /// Форма додавання / редагування заняття в розкладі.
    /// </summary>
    public partial class LessonForm : EditFormBase
    {
        private readonly Lesson _lesson;

        public LessonForm(SchoolService service, Lesson lesson, int defaultGroupId)
            : base(service, lesson == null, "заняття")
        {
            InitializeComponent();
            _lesson = lesson == null ? new Lesson { GroupId = defaultGroupId } : lesson.Clone<Lesson>();
            LoadData();

            cmbGroup.SelectedIndexChanged += (s, e) => OnGroupChanged();
            UpdateTeacherInfo();
        }

        private void LoadData()
        {
            UiHelper.Fill(cmbGroup, Service.Groups.GetAll().OrderBy(g => g.Name)
                .Select(g => new ComboItem(g.Id, g.Name + " — " + Service.GetCourseName(g.CourseId))), _lesson.GroupId);
            UiHelper.Fill(cmbDay, DayNames.GetItems(), _lesson.Day);
            dtpStartTime.Value = DateTime.Today.Add(_lesson.StartTime);
            numDuration.Value = _lesson.DurationMinutes;
            txtRoom.Text = _lesson.Room;
            if (IsNew)
                OnGroupChanged();
        }

        private void OnGroupChanged()
        {
            // Для зручності підставляємо аудиторію групи за замовчуванням
            Group group = Service.Groups.GetById(UiHelper.GetValue<int>(cmbGroup));
            if (group != null && (IsNew || txtRoom.Text.Length == 0))
                txtRoom.Text = group.Room;
            UpdateTeacherInfo();
        }

        private void UpdateTeacherInfo()
        {
            Teacher teacher = Service.GetTeacherOfGroup(UiHelper.GetValue<int>(cmbGroup));
            lblTeacher.Text = teacher == null ? "Викладач: —" : "Викладач: " + teacher.ShortName;
        }

        protected override void SaveEntity()
        {
            _lesson.GroupId = UiHelper.GetValue<int>(cmbGroup);
            _lesson.Day = UiHelper.GetValue<DayOfWeek>(cmbDay);
            _lesson.StartTime = new TimeSpan(dtpStartTime.Value.Hour, dtpStartTime.Value.Minute, 0);
            _lesson.DurationMinutes = (int)numDuration.Value;
            _lesson.Room = txtRoom.Text;

            if (IsNew)
                Service.AddLesson(_lesson);
            else
                Service.UpdateLesson(_lesson);
        }
    }
}
