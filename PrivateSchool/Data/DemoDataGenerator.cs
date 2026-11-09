using System;
using System.Collections.Generic;
using System.Linq;
using PrivateSchool.Models;
using PrivateSchool.Services;

namespace PrivateSchool.Data
{
    /// <summary>
    /// Заповнює порожню базу демонстраційними даними при першому запуску програми.
    /// </summary>
    public static class DemoDataGenerator
    {
        private static readonly string[] LastNames =
        {
            "Шевченко", "Коваль", "Бондар", "Ткачук", "Кравченко", "Олійник", "Лисенко", "Мороз",
            "Савчук", "Руденко", "Гончаренко", "Павленко", "Марченко", "Левченко", "Поліщук", "Кузьменко",
            "Захарченко", "Литвиненко", "Остапчук", "Гнатюк", "Сидоренко", "Петренко", "Демченко", "Романюк",
            "Климченко", "Ярошенко", "Дорошенко", "Федорчук"
        };

        private static readonly string[] MaleNames =
        {
            "Олександр", "Максим", "Артем", "Дмитро", "Назар", "Богдан", "Марко", "Іван", "Тимофій", "Данило"
        };

        private static readonly string[] FemaleNames =
        {
            "Софія", "Анна", "Марія", "Вікторія", "Дарина", "Соломія", "Злата", "Катерина", "Олена", "Ярослава"
        };

        private static readonly string[] MotherNames =
        {
            "Оксана", "Наталія", "Тетяна", "Ірина", "Світлана", "Юлія", "Людмила", "Галина"
        };

        // По батькові: (чоловіча форма, жіноча форма)
        private static readonly string[][] Patronymics =
        {
            new[] { "Олександрович", "Олександрівна" },
            new[] { "Сергійович", "Сергіївна" },
            new[] { "Андрійович", "Андріївна" },
            new[] { "Віталійович", "Віталіївна" },
            new[] { "Миколайович", "Миколаївна" },
            new[] { "Ігорович", "Ігорівна" },
            new[] { "Петрович", "Петрівна" },
            new[] { "Васильович", "Василівна" }
        };

        public static void Fill(SchoolData data)
        {
            Random random = new Random(2026);
            DateTime today = DateTime.Today;
            DateTime academicStart = new DateTime(today.Month >= 9 ? today.Year : today.Year - 1, 9, 1);

            AddUsers(data);
            AddCourses(data);
            AddTeachers(data);
            AddGroups(data, academicStart);
            AddLessons(data);
            AddStudents(data, random, academicStart);
            AddPayments(data, random, today);
            AddGrades(data, random, today);
        }

        private static void AddUsers(SchoolData data)
        {
            data.Users.Add(new User
            {
                Id = 1, Login = "admin", FullName = "Адміністратор системи", Role = UserRole.Administrator,
                PasswordHash = PasswordHasher.Hash("admin", "admin123")
            });
            data.Users.Add(new User
            {
                Id = 2, Login = "manager", FullName = "Литвин Ольга Сергіївна", Role = UserRole.Manager,
                PasswordHash = PasswordHasher.Hash("manager", "manager123")
            });
        }

        private static void AddCourses(SchoolData data)
        {
            data.Courses.Add(new Course
            {
                Id = 1, Name = "Англійська мова (A2–B1)", Level = CourseLevel.Intermediate, PricePerMonth = 2200,
                DurationMonths = 9, LessonsPerWeek = 2,
                Description = "Загальний курс англійської мови для школярів 12–15 років. Граматика, розмовна практика, підготовка до міжнародних іспитів."
            });
            data.Courses.Add(new Course
            {
                Id = 2, Name = "Математика: підготовка до НМТ", Level = CourseLevel.Advanced, PricePerMonth = 2500,
                DurationMonths = 9, LessonsPerWeek = 2,
                Description = "Систематизація шкільного курсу алгебри та геометрії, розв'язування тестових завдань НМТ."
            });
            data.Courses.Add(new Course
            {
                Id = 3, Name = "Програмування на Python для дітей", Level = CourseLevel.Beginner, PricePerMonth = 2800,
                DurationMonths = 8, LessonsPerWeek = 2,
                Description = "Основи алгоритмізації та програмування мовою Python, створення простих ігор."
            });
            data.Courses.Add(new Course
            {
                Id = 4, Name = "Українська мова та література", Level = CourseLevel.Intermediate, PricePerMonth = 2000,
                DurationMonths = 9, LessonsPerWeek = 2,
                Description = "Поглиблене вивчення української мови та літератури, підготовка до олімпіад і НМТ."
            });
            data.Courses.Add(new Course
            {
                Id = 5, Name = "Англійська для малюків", Level = CourseLevel.Beginner, PricePerMonth = 1800,
                DurationMonths = 9, LessonsPerWeek = 2,
                Description = "Ігровий курс англійської мови для дітей 6–8 років."
            });
        }

        private static void AddTeachers(SchoolData data)
        {
            data.Teachers.Add(CreateTeacher(1, "Коваленко", "Олена", "Петрівна", new DateTime(1985, 4, 12),
                "Англійська мова", "КНУ ім. Т. Шевченка, філологія", TeacherCategory.Highest, 15, 320));
            data.Teachers.Add(CreateTeacher(2, "Мельник", "Андрій", "Васильович", new DateTime(1979, 10, 3),
                "Математика", "НПУ ім. М. Драгоманова, математика", TeacherCategory.First, 20, 350));
            data.Teachers.Add(CreateTeacher(3, "Шевчук", "Дмитро", "Олегович", new DateTime(1994, 1, 25),
                "Програмування", "КПІ ім. Ігоря Сікорського, комп'ютерні науки", TeacherCategory.Second, 6, 380));
            data.Teachers.Add(CreateTeacher(4, "Бондаренко", "Ірина", "Миколаївна", new DateTime(1982, 7, 18),
                "Українська мова та література", "КНУ ім. Т. Шевченка, українська філологія", TeacherCategory.Highest, 18, 300));
            data.Teachers.Add(CreateTeacher(5, "Ткаченко", "Марія", "Іванівна", new DateTime(1997, 3, 9),
                "Англійська мова", "Київський університет ім. Б. Грінченка, філологія", TeacherCategory.Specialist, 3, 260));
        }

        private static Teacher CreateTeacher(int id, string last, string first, string middle, DateTime birth,
            string specialization, string education, TeacherCategory category, int experience, decimal rate)
        {
            return new Teacher
            {
                Id = id, LastName = last, FirstName = first, MiddleName = middle, BirthDate = birth,
                Specialization = specialization, Education = education, Category = category,
                ExperienceYears = experience, HourlyRate = rate,
                HireDate = DateTime.Today.AddYears(-Math.Min(experience, 5)).AddMonths(-2),
                Phone = "+38067" + (1000000 + id * 1234567 % 8999999),
                Email = TransliterateEmail(last) + "@znannia.ua"
            };
        }

        private static string TransliterateEmail(string lastName)
        {
            const string ua = "абвгґдеєжзиіїйклмнопрстуфхцчшщьюя'";
            string[] en =
            {
                "a", "b", "v", "h", "g", "d", "e", "ie", "zh", "z", "y", "i", "i", "i", "k", "l", "m", "n", "o", "p",
                "r", "s", "t", "u", "f", "kh", "ts", "ch", "sh", "shch", "", "iu", "ia", ""
            };
            return string.Concat(lastName.ToLowerInvariant().Select(c =>
            {
                int index = ua.IndexOf(c);
                return index >= 0 ? en[index] : c.ToString();
            }));
        }

        private static void AddGroups(SchoolData data, DateTime start)
        {
            data.Groups.Add(new Group { Id = 1, Name = "ENG-1", CourseId = 1, TeacherId = 1, StartDate = start, MaxStudents = 10, Room = "101" });
            data.Groups.Add(new Group { Id = 2, Name = "ENG-2", CourseId = 1, TeacherId = 1, StartDate = start.AddMonths(1), MaxStudents = 10, Room = "101" });
            data.Groups.Add(new Group { Id = 3, Name = "НМТ-МАТ-11", CourseId = 2, TeacherId = 2, StartDate = start, MaxStudents = 12, Room = "102" });
            data.Groups.Add(new Group { Id = 4, Name = "PY-KIDS-1", CourseId = 3, TeacherId = 3, StartDate = start, MaxStudents = 8, Room = "201 (комп. клас)" });
            data.Groups.Add(new Group { Id = 5, Name = "УКР-9", CourseId = 4, TeacherId = 4, StartDate = start, MaxStudents = 12, Room = "103" });
            data.Groups.Add(new Group { Id = 6, Name = "KIDS-ENG", CourseId = 5, TeacherId = 5, StartDate = start, MaxStudents = 8, Room = "104" });
        }

        private static void AddLessons(SchoolData data)
        {
            int id = 1;
            Action<int, DayOfWeek, int, int, string> add = (groupId, day, hour, minute, room) =>
                data.Lessons.Add(new Lesson
                {
                    Id = id++, GroupId = groupId, Day = day, StartTime = new TimeSpan(hour, minute, 0),
                    DurationMinutes = groupId == 6 ? 60 : 90, Room = room
                });

            add(1, DayOfWeek.Monday, 15, 0, "101");
            add(1, DayOfWeek.Wednesday, 15, 0, "101");
            add(2, DayOfWeek.Tuesday, 16, 30, "101");
            add(2, DayOfWeek.Thursday, 16, 30, "101");
            add(3, DayOfWeek.Tuesday, 15, 0, "102");
            add(3, DayOfWeek.Friday, 15, 0, "102");
            add(4, DayOfWeek.Monday, 16, 0, "201 (комп. клас)");
            add(4, DayOfWeek.Thursday, 16, 0, "201 (комп. клас)");
            add(5, DayOfWeek.Wednesday, 17, 0, "103");
            add(5, DayOfWeek.Friday, 17, 0, "103");
            add(6, DayOfWeek.Saturday, 10, 0, "104");
            add(6, DayOfWeek.Saturday, 11, 30, "104");
        }

        private static void AddStudents(SchoolData data, Random random, DateTime academicStart)
        {
            // Вік учнів для кожної групи: (мінімальний, максимальний)
            Dictionary<int, int[]> ages = new Dictionary<int, int[]>
            {
                { 1, new[] { 12, 15 } }, { 2, new[] { 12, 15 } }, { 3, new[] { 16, 17 } },
                { 4, new[] { 9, 12 } }, { 5, new[] { 14, 15 } }, { 6, new[] { 6, 8 } }
            };
            int[] perGroup = { 5, 4, 6, 5, 4, 4 };

            int id = 1;
            int lastNameIndex = 0;
            for (int groupId = 1; groupId <= 6; groupId++)
            {
                Group group = data.Groups.First(g => g.Id == groupId);
                for (int i = 0; i < perGroup[groupId - 1]; i++)
                {
                    bool male = random.Next(2) == 0;
                    string[] patronymic = Patronymics[random.Next(Patronymics.Length)];
                    string lastName = LastNames[lastNameIndex++ % LastNames.Length];
                    int age = random.Next(ages[groupId][0], ages[groupId][1] + 1);

                    Student student = new Student
                    {
                        Id = id,
                        LastName = lastName,
                        FirstName = male ? MaleNames[random.Next(MaleNames.Length)] : FemaleNames[random.Next(FemaleNames.Length)],
                        MiddleName = male ? patronymic[0] : patronymic[1],
                        BirthDate = DateTime.Today.AddYears(-age).AddDays(-random.Next(30, 330)),
                        Phone = age >= 12 ? "+38050" + random.Next(1000000, 9999999) : string.Empty,
                        ContractNumber = string.Format("Д-{0}/{1:000}", academicStart.Year % 100, id),
                        EnrollmentDate = group.StartDate.AddDays(random.Next(0, 10) < 8 ? 0 : random.Next(20, 45)),
                        GroupId = groupId,
                        Discount = random.Next(10) == 0 ? 20 : (random.Next(8) == 0 ? 10 : 0),
                        ParentName = lastName + " " + MotherNames[random.Next(MotherNames.Length)] + " " +
                                     Patronymics[random.Next(Patronymics.Length)][1],
                        ParentPhone = "+38063" + random.Next(1000000, 9999999)
                    };
                    data.Students.Add(student);
                    id++;
                }
            }

            // Один учень в академвідпустці — для демонстрації статусів
            data.Students[7].Status = StudentStatus.Suspended;
        }

        private static void AddPayments(SchoolData data, Random random, DateTime today)
        {
            int id = 1;
            foreach (Student student in data.Students)
            {
                Group group = data.Groups.First(g => g.Id == student.GroupId);
                Course course = data.Courses.First(c => c.Id == group.CourseId);
                decimal fee = student.CalculateMonthlyFee(course);

                DateTime start = student.EnrollmentDate > group.StartDate ? student.EnrollmentDate : group.StartDate;
                DateTime month = new DateTime(start.Year, start.Month, 1);

                // Приблизно кожен четвертий учень не сплатив за останній місяць
                bool debtor = random.Next(4) == 0;
                DateTime lastPaidMonth = debtor ? new DateTime(today.Year, today.Month, 1).AddMonths(-1)
                                                : new DateTime(today.Year, today.Month, 1);
                if (student.Status == StudentStatus.Suspended)
                    lastPaidMonth = month.AddMonths(1);

                while (month <= lastPaidMonth)
                {
                    DateTime date = month.AddDays(random.Next(0, 7));
                    if (date < start) date = start;
                    if (date > today) date = today;

                    data.Payments.Add(new Payment
                    {
                        Id = id++,
                        StudentId = student.Id,
                        Date = date,
                        Amount = fee,
                        Method = (PaymentMethod)random.Next(3),
                        Comment = "Оплата за " + month.ToString("MMMM yyyy", new System.Globalization.CultureInfo("uk-UA"))
                    });
                    month = month.AddMonths(1);
                }
            }
        }

        private static void AddGrades(SchoolData data, Random random, DateTime today)
        {
            int id = 1;
            foreach (Student student in data.Students)
            {
                Group group = data.Groups.First(g => g.Id == student.GroupId);
                DateTime start = student.EnrollmentDate > group.StartDate ? student.EnrollmentDate : group.StartDate;
                int days = Math.Max(1, (today - start).Days);
                int ability = random.Next(5, 12); // «рівень» учня, навколо якого коливаються оцінки
                int count = Math.Min(10, 3 + days / 10);

                for (int i = 0; i < count; i++)
                {
                    int value = Math.Max(1, Math.Min(12, ability + random.Next(-2, 3)));
                    data.Grades.Add(new Grade
                    {
                        Id = id++,
                        StudentId = student.Id,
                        GroupId = group.Id,
                        Date = start.AddDays(random.Next(days)),
                        Value = value,
                        Kind = i % 4 == 3 ? GradeKind.Test : GradeKind.Current,
                        Comment = i % 4 == 3 ? "Контрольна робота" : string.Empty
                    });
                }
            }
        }
    }
}
