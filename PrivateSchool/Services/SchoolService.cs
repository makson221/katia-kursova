using System;
using System.Collections.Generic;
using System.Linq;
using PrivateSchool.Data;
using PrivateSchool.Models;

namespace PrivateSchool.Services
{
    /// <summary>
    /// Головний сервіс бізнес-логіки (фасад): надає доступ до репозиторіїв,
    /// перевіряє бізнес-правила, рахує фінанси та успішність, зберігає дані.
    /// </summary>
    public class SchoolService
    {
        private readonly IDataStorage _storage;
        private SchoolData _data;
        private bool _suspendSaving;

        /// <summary>Подія, що виникає після будь-якої зміни даних (для оновлення інтерфейсу).</summary>
        public event EventHandler DataChanged;

        public SchoolService(IDataStorage storage)
        {
            if (storage == null)
                throw new ArgumentNullException("storage");
            _storage = storage;
            Load();
        }

        public Repository<Student> Students { get; private set; }
        public Repository<Teacher> Teachers { get; private set; }
        public Repository<Course> Courses { get; private set; }
        public Repository<Group> Groups { get; private set; }
        public Repository<Lesson> Lessons { get; private set; }
        public Repository<Payment> Payments { get; private set; }
        public Repository<Grade> Grades { get; private set; }
        public Repository<User> Users { get; private set; }

        public string SchoolName
        {
            get { return _data.SchoolName; }
        }

        public string StorageLocation
        {
            get { return _storage.Location; }
        }

        #region Завантаження та збереження

        private void Load()
        {
            _data = _storage.Load();
            if (_data.IsEmpty)
            {
                DemoDataGenerator.Fill(_data);
                _storage.Save(_data);
            }

            Students = CreateRepository(_data.Students);
            Teachers = CreateRepository(_data.Teachers);
            Courses = CreateRepository(_data.Courses);
            Groups = CreateRepository(_data.Groups);
            Lessons = CreateRepository(_data.Lessons);
            Payments = CreateRepository(_data.Payments);
            Grades = CreateRepository(_data.Grades);
            Users = CreateRepository(_data.Users);
        }

        private Repository<T> CreateRepository<T>(List<T> list) where T : EntityBase
        {
            Repository<T> repository = new Repository<T>(list);
            repository.Changed += OnRepositoryChanged;
            return repository;
        }

        private void OnRepositoryChanged(object sender, EventArgs e)
        {
            if (_suspendSaving)
                return;
            Save();
            EventHandler handler = DataChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public void Save()
        {
            _storage.Save(_data);
        }

        /// <summary>Виконує кілька змін як одну операцію: зберігає й сповіщає лише один раз.</summary>
        private void Batch(Action action)
        {
            _suspendSaving = true;
            try
            {
                action();
            }
            finally
            {
                _suspendSaving = false;
            }
            OnRepositoryChanged(this, EventArgs.Empty);
        }

        #endregion

        #region Довідкові методи

        public Course GetCourseOfGroup(int groupId)
        {
            Group group = Groups.GetById(groupId);
            return group == null ? null : Courses.GetById(group.CourseId);
        }

        public Teacher GetTeacherOfGroup(int groupId)
        {
            Group group = Groups.GetById(groupId);
            return group == null ? null : Teachers.GetById(group.TeacherId);
        }

        public string GetGroupName(int groupId)
        {
            Group group = Groups.GetById(groupId);
            return group == null ? "—" : group.Name;
        }

        public string GetStudentName(int studentId)
        {
            Student student = Students.GetById(studentId);
            return student == null ? "(видалений учень)" : student.FullName;
        }

        public string GetTeacherName(int teacherId)
        {
            Teacher teacher = Teachers.GetById(teacherId);
            return teacher == null ? "—" : teacher.ShortName;
        }

        public string GetCourseName(int courseId)
        {
            Course course = Courses.GetById(courseId);
            return course == null ? "—" : course.Name;
        }

        public IEnumerable<Student> GetStudentsOfGroup(int groupId)
        {
            return Students.Find(s => s.GroupId == groupId).OrderBy(s => s.FullName);
        }

        public int GetActiveStudentCount(int groupId)
        {
            return Students.Find(s => s.GroupId == groupId && s.IsActive).Count();
        }

        public IEnumerable<Group> GetGroupsOfTeacher(int teacherId)
        {
            return Groups.Find(g => g.TeacherId == teacherId);
        }

        /// <summary>Розклад, відсортований за днем тижня та часом.</summary>
        public IEnumerable<Lesson> GetSchedule(Func<Lesson, bool> filter = null)
        {
            IEnumerable<Lesson> lessons = filter == null ? Lessons.GetAll() : Lessons.Find(filter);
            return lessons.OrderBy(l => Utils.DayNames.OrderOf(l.Day)).ThenBy(l => l.StartTime);
        }

        /// <summary>Тижневе навантаження викладача в академічних годинах (по 60 хв).</summary>
        public double GetTeacherWeeklyHours(int teacherId)
        {
            HashSet<int> groupIds = new HashSet<int>(GetGroupsOfTeacher(teacherId).Select(g => g.Id));
            return Lessons.Find(l => groupIds.Contains(l.GroupId)).Sum(l => l.DurationMinutes) / 60.0;
        }

        #endregion

        #region Учні

        public void AddStudent(Student student)
        {
            CheckStudentRules(student, true);
            Students.Add(student);
        }

        public void UpdateStudent(Student student)
        {
            CheckStudentRules(student, false);
            Students.Update(student);
        }

        private void CheckStudentRules(Student student, bool isNew)
        {
            student.Validate();

            bool duplicateContract = Students.Find(s => s.Id != student.Id
                && string.Equals(s.ContractNumber, student.ContractNumber, StringComparison.OrdinalIgnoreCase)).Any();
            if (duplicateContract)
                throw new ValidationException("Учень із договором № " + student.ContractNumber + " вже існує.");

            if (student.GroupId == 0)
                return;

            Group group = Groups.GetById(student.GroupId);
            if (group == null)
                throw new ValidationException("Обрана група не існує.");

            Student old = isNew ? null : Students.GetById(student.Id);
            bool joinsGroup = old == null || old.GroupId != student.GroupId || (!old.IsActive && student.IsActive);
            if (student.IsActive && joinsGroup && GetActiveStudentCount(group.Id) >= group.MaxStudents)
                throw new ValidationException(string.Format(
                    "У групі «{0}» немає вільних місць (максимум {1}).", group.Name, group.MaxStudents));
        }

        /// <summary>Видаляє учня разом із його оцінками та оплатами.</summary>
        public void DeleteStudent(int studentId)
        {
            Batch(() =>
            {
                Grades.RemoveWhere(g => g.StudentId == studentId);
                Payments.RemoveWhere(p => p.StudentId == studentId);
                Students.Remove(studentId);
            });
        }

        #endregion

        #region Викладачі

        public void AddTeacher(Teacher teacher)
        {
            Teachers.Add(teacher);
        }

        public void UpdateTeacher(Teacher teacher)
        {
            Teachers.Update(teacher);
        }

        public void DeleteTeacher(int teacherId)
        {
            Group group = GetGroupsOfTeacher(teacherId).FirstOrDefault();
            if (group != null)
                throw new ValidationException(string.Format(
                    "Викладач веде групу «{0}». Спочатку призначте групі іншого викладача.", group.Name));
            Teachers.Remove(teacherId);
        }

        #endregion

        #region Курси

        public void AddCourse(Course course)
        {
            CheckCourseName(course);
            Courses.Add(course);
        }

        public void UpdateCourse(Course course)
        {
            CheckCourseName(course);
            Courses.Update(course);
        }

        private void CheckCourseName(Course course)
        {
            course.Validate();
            if (Courses.Find(c => c.Id != course.Id
                    && string.Equals(c.Name, course.Name, StringComparison.CurrentCultureIgnoreCase)).Any())
                throw new ValidationException("Курс із назвою «" + course.Name + "» вже існує.");
        }

        public void DeleteCourse(int courseId)
        {
            Group group = Groups.Find(g => g.CourseId == courseId).FirstOrDefault();
            if (group != null)
                throw new ValidationException(string.Format(
                    "За курсом навчається група «{0}». Видалення неможливе.", group.Name));
            Courses.Remove(courseId);
        }

        #endregion

        #region Групи

        public void AddGroup(Group group)
        {
            CheckGroupRules(group);
            Groups.Add(group);
        }

        public void UpdateGroup(Group group)
        {
            CheckGroupRules(group);
            Groups.Update(group);
        }

        private void CheckGroupRules(Group group)
        {
            group.Validate();
            if (Courses.GetById(group.CourseId) == null)
                throw new ValidationException("Обраний курс не існує.");
            if (Teachers.GetById(group.TeacherId) == null)
                throw new ValidationException("Обраний викладач не існує.");
            if (Groups.Find(g => g.Id != group.Id
                    && string.Equals(g.Name, group.Name, StringComparison.CurrentCultureIgnoreCase)).Any())
                throw new ValidationException("Група з назвою «" + group.Name + "» вже існує.");

            int active = GetActiveStudentCount(group.Id);
            if (group.Id > 0 && group.MaxStudents < active)
                throw new ValidationException(string.Format(
                    "У групі вже навчається {0} учнів — кількість місць не може бути меншою.", active));

            // Перевірка: новий викладач не повинен мати накладок у розкладі цієї групи
            foreach (Lesson lesson in Lessons.Find(l => l.GroupId == group.Id))
                CheckLessonConflicts(lesson, group);
        }

        /// <summary>Видаляє групу та її розклад. Групу з учнями видалити не можна.</summary>
        public void DeleteGroup(int groupId)
        {
            if (Students.Find(s => s.GroupId == groupId).Any())
                throw new ValidationException("У групі є учні. Спочатку переведіть їх до іншої групи.");
            Batch(() =>
            {
                Lessons.RemoveWhere(l => l.GroupId == groupId);
                Grades.RemoveWhere(g => g.GroupId == groupId);
                Groups.Remove(groupId);
            });
        }

        #endregion

        #region Розклад

        public void AddLesson(Lesson lesson)
        {
            lesson.Validate();
            CheckLessonConflicts(lesson, Groups.GetById(lesson.GroupId));
            Lessons.Add(lesson);
        }

        public void UpdateLesson(Lesson lesson)
        {
            lesson.Validate();
            CheckLessonConflicts(lesson, Groups.GetById(lesson.GroupId));
            Lessons.Update(lesson);
        }

        public void DeleteLesson(int lessonId)
        {
            Lessons.Remove(lessonId);
        }

        /// <summary>
        /// Перевіряє накладки в розкладі: одна група, один викладач або одна аудиторія
        /// не можуть мати два заняття одночасно.
        /// </summary>
        private void CheckLessonConflicts(Lesson lesson, Group group)
        {
            if (group == null)
                throw new ValidationException("Обрана група не існує.");

            foreach (Lesson other in Lessons.Find(l => l.Id != lesson.Id && l.OverlapsWith(lesson)))
            {
                Group otherGroup = Groups.GetById(other.GroupId);
                string when = Utils.DayNames.ToText(other.Day) + " " + other.TimeRange;

                if (other.GroupId == lesson.GroupId)
                    throw new ValidationException("Група вже має заняття в цей час: " + when + ".");
                if (otherGroup != null && otherGroup.TeacherId == group.TeacherId)
                    throw new ValidationException(string.Format(
                        "Викладач {0} у цей час веде заняття групи «{1}» ({2}).",
                        GetTeacherName(group.TeacherId), otherGroup.Name, when));
                if (string.Equals(other.Room, lesson.Room, StringComparison.CurrentCultureIgnoreCase))
                    throw new ValidationException(string.Format(
                        "Аудиторія {0} зайнята групою «{1}» ({2}).",
                        lesson.Room, otherGroup == null ? "?" : otherGroup.Name, when));
            }
        }

        #endregion

        #region Оплати

        public void AddPayment(Payment payment)
        {
            CheckStudentExists(payment.StudentId);
            Payments.Add(payment);
        }

        public void UpdatePayment(Payment payment)
        {
            CheckStudentExists(payment.StudentId);
            Payments.Update(payment);
        }

        public void DeletePayment(int paymentId)
        {
            Payments.Remove(paymentId);
        }

        private void CheckStudentExists(int studentId)
        {
            if (Students.GetById(studentId) == null)
                throw new ValidationException("Обраний учень не існує.");
        }

        /// <summary>Щомісячна оплата учня з урахуванням знижки.</summary>
        public decimal GetMonthlyFee(Student student)
        {
            return student.CalculateMonthlyFee(GetCourseOfGroup(student.GroupId));
        }

        /// <summary>
        /// Кількість місяців, за які нараховано оплату: з місяця початку навчання
        /// (пізніша з дат зарахування та старту групи) по поточний місяць включно,
        /// але не більше тривалості курсу.
        /// </summary>
        public int GetBilledMonths(Student student, DateTime asOf)
        {
            Group group = Groups.GetById(student.GroupId);
            Course course = group == null ? null : Courses.GetById(group.CourseId);
            if (course == null)
                return 0;

            DateTime start = student.EnrollmentDate > group.StartDate ? student.EnrollmentDate : group.StartDate;
            if (start.Date > asOf.Date)
                return 0;

            int months = (asOf.Year - start.Year) * 12 + asOf.Month - start.Month + 1;
            return Math.Min(months, course.DurationMonths);
        }

        public decimal GetCharged(Student student)
        {
            if (student.Status == StudentStatus.Expelled || student.Status == StudentStatus.Suspended)
                return GetPaid(student); // нарахування зупинено — боргу немає
            return GetBilledMonths(student, DateTime.Today) * GetMonthlyFee(student);
        }

        public decimal GetPaid(Student student)
        {
            return Payments.Find(p => p.StudentId == student.Id).Sum(p => p.Amount);
        }

        /// <summary>Заборгованість учня (від'ємне значення — переплата).</summary>
        public decimal GetDebt(Student student)
        {
            return GetCharged(student) - GetPaid(student);
        }

        public decimal GetIncome(DateTime from, DateTime to)
        {
            return Payments.Find(p => p.Date.Date >= from.Date && p.Date.Date <= to.Date).Sum(p => p.Amount);
        }

        #endregion

        #region Оцінки

        public void AddGrade(Grade grade)
        {
            CheckGradeRules(grade);
            Grades.Add(grade);
        }

        public void UpdateGrade(Grade grade)
        {
            CheckGradeRules(grade);
            Grades.Update(grade);
        }

        public void DeleteGrade(int gradeId)
        {
            Grades.Remove(gradeId);
        }

        private void CheckGradeRules(Grade grade)
        {
            grade.Validate();
            Student student = Students.GetById(grade.StudentId);
            if (student == null)
                throw new ValidationException("Обраний учень не існує.");
            if (student.GroupId != grade.GroupId)
                throw new ValidationException("Учень не навчається в обраній групі.");
        }

        /// <summary>Середній бал (null, якщо оцінок немає).</summary>
        public double? GetAverageGrade(Func<Grade, bool> filter)
        {
            List<Grade> grades = Grades.Find(filter).ToList();
            if (grades.Count == 0)
                return null;
            return Math.Round(grades.Average(g => g.Value), 2);
        }

        public double? GetStudentAverage(int studentId)
        {
            return GetAverageGrade(g => g.StudentId == studentId);
        }

        public double? GetGroupAverage(int groupId)
        {
            return GetAverageGrade(g => g.GroupId == groupId);
        }

        #endregion

        #region Користувачі

        public void AddUser(User user, string password)
        {
            CheckPassword(password);
            user.PasswordHash = PasswordHasher.Hash(user.Login, password);
            CheckUserRules(user);
            Users.Add(user);
        }

        /// <summary>Оновлює користувача; якщо пароль порожній — залишається попередній.</summary>
        public void UpdateUser(User user, string newPassword)
        {
            User old = Users.GetById(user.Id);
            if (old == null)
                throw new ValidationException("Користувача не знайдено.");

            if (!string.IsNullOrEmpty(newPassword))
            {
                CheckPassword(newPassword);
                user.PasswordHash = PasswordHasher.Hash(user.Login, newPassword);
            }
            else if (old.Login != user.Login)
            {
                throw new ValidationException("При зміні логіну необхідно задати новий пароль.");
            }

            if (old.IsAdministrator && !user.IsAdministrator && CountAdministrators() == 1)
                throw new ValidationException("У системі має залишитися хоча б один адміністратор.");

            CheckUserRules(user);
            Users.Update(user);
        }

        public void DeleteUser(int userId, User currentUser)
        {
            User user = Users.GetById(userId);
            if (user == null)
                return;
            if (currentUser != null && user.Id == currentUser.Id)
                throw new ValidationException("Не можна видалити обліковий запис, під яким виконано вхід.");
            if (user.IsAdministrator && CountAdministrators() == 1)
                throw new ValidationException("Не можна видалити єдиного адміністратора.");
            Users.Remove(userId);
        }

        private int CountAdministrators()
        {
            return Users.Find(u => u.IsAdministrator).Count();
        }

        private void CheckUserRules(User user)
        {
            user.Validate();
            if (Users.Find(u => u.Id != user.Id && u.Login == user.Login).Any())
                throw new ValidationException("Користувач із логіном «" + user.Login + "» вже існує.");
        }

        private static void CheckPassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 6)
                throw new ValidationException("Пароль має містити щонайменше 6 символів.");
        }

        #endregion
    }
}
