using System.Linq;
using PrivateSchool.Models;
using PrivateSchool.Services;
using PrivateSchool.Utils;

namespace PrivateSchool.Forms
{
    /// <summary>
    /// Форма додавання / редагування навчальної групи.
    /// </summary>
    public partial class GroupForm : EditFormBase
    {
        private readonly Group _group;

        public GroupForm(SchoolService service, Group group)
            : base(service, group == null, "група")
        {
            InitializeComponent();
            _group = group == null ? new Group() : group.Clone<Group>();
            LoadData();
        }

        private void LoadData()
        {
            txtName.Text = _group.Name;
            txtRoom.Text = _group.Room;
            dtpStartDate.Value = _group.StartDate;
            numMaxStudents.Value = _group.MaxStudents;

            UiHelper.Fill(cmbCourse, Service.Courses.GetAll().OrderBy(c => c.Name)
                .Select(c => new ComboItem(c.Id, c.Name)), _group.CourseId);
            UiHelper.Fill(cmbTeacher, Service.Teachers.GetAll().OrderBy(t => t.FullName)
                .Select(t => new ComboItem(t.Id, t.ShortName + " — " + t.Specialization)), _group.TeacherId);
        }

        protected override void SaveEntity()
        {
            _group.Name = txtName.Text;
            _group.Room = txtRoom.Text;
            _group.StartDate = dtpStartDate.Value.Date;
            _group.MaxStudents = (int)numMaxStudents.Value;
            _group.CourseId = UiHelper.GetValue<int>(cmbCourse);
            _group.TeacherId = UiHelper.GetValue<int>(cmbTeacher);

            if (IsNew)
                Service.AddGroup(_group);
            else
                Service.UpdateGroup(_group);
        }
    }
}
