using System.Linq;
using PrivateSchool.Models;
using PrivateSchool.Services;
using PrivateSchool.Utils;

namespace PrivateSchool.Forms
{
    /// <summary>
    /// Форма виставлення / редагування оцінки.
    /// </summary>
    public partial class GradeForm : EditFormBase
    {
        private readonly Grade _grade;

        public GradeForm(SchoolService service, Grade grade, int defaultGroupId)
            : base(service, grade == null, "оцінка")
        {
            InitializeComponent();
            _grade = grade == null ? new Grade { GroupId = defaultGroupId } : grade.Clone<Grade>();
            dtpDate.MaxDate = System.DateTime.Today;
            LoadData();

            cmbGroup.SelectedIndexChanged += (s, e) => FillStudents(0);
            numValue.ValueChanged += (s, e) => UpdateLevel();
            UpdateLevel();
        }

        private void LoadData()
        {
            UiHelper.Fill(cmbGroup, Service.Groups.GetAll().OrderBy(g => g.Name)
                .Select(g => new ComboItem(g.Id, g.Name + " — " + Service.GetCourseName(g.CourseId))), _grade.GroupId);
            FillStudents(_grade.StudentId);
            UiHelper.Fill(cmbKind, EnumHelper.GetItems<GradeKind>(), _grade.Kind);
            dtpDate.Value = _grade.Date > System.DateTime.Today ? System.DateTime.Today : _grade.Date;
            numValue.Value = _grade.Value;
            txtComment.Text = _grade.Comment;
        }

        /// <summary>Показує лише учнів обраної групи.</summary>
        private void FillStudents(int selectedStudentId)
        {
            int groupId = UiHelper.GetValue<int>(cmbGroup);
            UiHelper.Fill(cmbStudent, Service.GetStudentsOfGroup(groupId)
                .Where(s => s.IsActive || s.Id == selectedStudentId)
                .Select(s => new ComboItem(s.Id, s.FullName)), selectedStudentId);
        }

        private void UpdateLevel()
        {
            lblLevel.Text = "Рівень досягнень: " + Grade.GetLevelName((double)numValue.Value);
        }

        protected override void SaveEntity()
        {
            _grade.GroupId = UiHelper.GetValue<int>(cmbGroup);
            _grade.StudentId = UiHelper.GetValue<int>(cmbStudent);
            _grade.Date = dtpDate.Value.Date;
            _grade.Value = (int)numValue.Value;
            _grade.Kind = UiHelper.GetValue<GradeKind>(cmbKind);
            _grade.Comment = txtComment.Text;

            if (IsNew)
                Service.AddGrade(_grade);
            else
                Service.UpdateGrade(_grade);
        }
    }
}
