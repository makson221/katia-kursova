using PrivateSchool.Core.Models;
using PrivateSchool.Core.Services;

namespace PrivateSchool.Core.Data;

/// <summary>
/// Генератор демонстраційних даних для першого запуску системи.
/// Використовує фіксоване зерно генератора, тож дані відтворювані.
/// </summary>
public static class SeedData
{
    private static readonly string[] LastNames =
    {
        "Коваль", "Мельничук", "Бойко", "Ткачук", "Поліщук", "Кравець", "Павленко", "Марченко", "Руденко",
        "Литвиненко", "Захарченко", "Романюк", "Гуменюк", "Юрченко", "Дячук", "Остапчук", "Василенко",
        "Петренко", "Левченко", "Карпенко", "Мороз", "Савчук", "Зінченко", "Онищенко", "Король", "Федоренко",
        "Сидоренко", "Клименко", "Демченко", "Кузьменко", "Гордієнко", "Тимошенко", "Хоменко", "Приходько"
    };

    private static readonly string[] MaleNames =
    {
        "Олександр", "Максим", "Артем", "Дмитро", "Назар", "Богдан", "Владислав", "Іван", "Андрій",
        "Тарас", "Микита", "Денис", "Ярослав", "Остап", "Роман", "Євген"
    };

    private static readonly string[] FemaleNames =
    {
        "Анастасія", "Софія", "Дарина", "Вікторія", "Юлія", "Катерина", "Олена", "Марія", "Аліна",
        "Ірина", "Яна", "Христина", "Діана", "Валерія", "Оксана", "Тетяна"
    };

    private static readonly (string Male, string Female, string Father)[] Patronymics =
    {
        ("Олександрович", "Олександрівна", "Олександр"), ("Сергійович", "Сергіївна", "Сергій"),
        ("Миколайович", "Миколаївна", "Микола"), ("Володимирович", "Володимирівна", "Володимир"),
        ("Андрійович", "Андріївна", "Андрій"), ("Іванович", "Іванівна", "Іван"),
        ("Петрович", "Петрівна", "Петро"), ("Васильович", "Василівна", "Василь"),
        ("Юрійович", "Юріївна", "Юрій"), ("Олегович", "Олегівна", "Олег"),
        ("Віталійович", "Віталіївна", "Віталій"), ("Богданович", "Богданівна", "Богдан")
    };

    private static readonly string[] PhonePrefixes = { "067", "050", "063", "093", "097", "066", "068", "073" };

    public static void Fill(SchoolDatabase db, AuthService auth)
    {
        var rnd = new Random(2024);
        var today = DateTime.Today;
        var academicYear = today.Month >= 9 ? today.Year : today.Year - 1;

        db.InstitutionName = "Приватний коледж «Освіта+»";

        var teachers = AddTeachers(db);
        var subjects = AddSubjects(db, teachers);
        var groups = AddGroups(db, teachers, academicYear);

        var curriculum = new Dictionary<Group, string[]>
        {
            [groups[0]] = new[] { "Об'єктно-орієнтоване програмування", "Бази даних", "Дискретна математика", "Англійська мова (професійна)", "Веб-технології", "Комп'ютерні мережі" },
            [groups[1]] = new[] { "Основи програмування", "Вища математика", "Дискретна математика", "Англійська мова (професійна)", "Українська мова за професійним спрямуванням", "Комп'ютерні мережі" },
            [groups[2]] = new[] { "Основи програмування", "Вища математика", "Англійська мова (професійна)", "Українська мова за професійним спрямуванням", "Веб-технології" },
            [groups[3]] = new[] { "Композиція та колористика", "Комп'ютерна графіка", "Англійська мова (професійна)", "Українська мова за професійним спрямуванням", "Веб-технології" },
            [groups[4]] = new[] { "Бухгалтерський облік", "Мікроекономіка", "Вища математика", "Англійська мова (професійна)", "Українська мова за професійним спрямуванням" }
        };

        AddSchedule(db, curriculum, subjects, rnd);

        var usedNames = new HashSet<string>();
        var contractNo = 1;
        var payments = new List<Payment>();
        var finance = new FinanceService(db);

        foreach (var group in groups)
        {
            var count = rnd.Next(12, 17);
            for (var i = 0; i < count; i++)
            {
                var student = CreateStudent(group, rnd, usedNames, ref contractNo, today);
                db.Students.Add(student);

                AddGrades(db, student, curriculum[group].Select(n => subjects[n]), rnd, today, academicYear);
                payments.AddRange(CreatePayments(student, finance.GetMonthlyFee(student), rnd, today));
            }
        }

        var counters = new Dictionary<int, int>();
        foreach (var payment in payments.OrderBy(p => p.Date))
        {
            counters.TryGetValue(payment.Date.Year, out var n);
            counters[payment.Date.Year] = ++n;
            payment.ReceiptNumber = $"КВ-{payment.Date.Year}-{n:D5}";
            db.Payments.Add(payment);
        }

        auth.CreateUser(new User { Login = "admin", DisplayName = "Адміністратор системи", Role = UserRole.Administrator }, "admin123");
        auth.CreateUser(new User { Login = "manager", DisplayName = "Петренко Ганна Іванівна", Role = UserRole.Manager }, "manager123");
        auth.CreateUser(new User { Login = "teacher", DisplayName = teachers[0].FullName, Role = UserRole.Teacher, TeacherId = teachers[0].Id }, "teacher123");
    }

    private static List<Teacher> AddTeachers(SchoolDatabase db)
    {
        var data = new (string Last, string First, string Middle, int Year, string Position, string Degree, string Spec, int Hired, decimal Rate)[]
        {
            ("Коваленко", "Олена", "Петрівна", 1981, "Доцент", "Кандидат технічних наук", "Програмування", 2012, 420),
            ("Бондаренко", "Андрій", "Миколайович", 1976, "Старший викладач", "", "Вища математика", 2008, 380),
            ("Мельник", "Ірина", "Василівна", 1988, "Викладач", "", "Англійська мова", 2015, 320),
            ("Шевчук", "Дмитро", "Олександрович", 1984, "Доцент", "Кандидат фізико-математичних наук", "Бази даних", 2013, 430),
            ("Ткаченко", "Наталія", "Іванівна", 1990, "Викладач", "", "Графічний дизайн", 2017, 350),
            ("Кравчук", "Сергій", "Володимирович", 1970, "Професор", "Доктор економічних наук", "Економіка", 2005, 500),
            ("Олійник", "Марія", "Степанівна", 1983, "Старший викладач", "", "Облік і аудит", 2011, 370),
            ("Лисенко", "Віктор", "Анатолійович", 1986, "Викладач", "", "Комп'ютерні мережі", 2016, 360),
            ("Савченко", "Юлія", "Романівна", 1992, "Викладач", "", "Українська мова", 2019, 300),
            ("Гончаренко", "Павло", "Ігорович", 1989, "Старший викладач", "", "Веб-технології", 2014, 400)
        };

        var result = new List<Teacher>();
        var i = 0;
        foreach (var d in data)
        {
            i++;
            result.Add(db.Teachers.Add(new Teacher
            {
                LastName = d.Last,
                FirstName = d.First,
                MiddleName = d.Middle,
                BirthDate = new DateTime(d.Year, 1 + i % 12, 3 + i * 2),
                Position = d.Position,
                AcademicDegree = d.Degree,
                Specialization = d.Spec,
                HireDate = new DateTime(d.Hired, 9, 1),
                HourlyRate = d.Rate,
                Phone = $"+38050{1234500 + i * 37:D7}",
                Email = $"{Transliterate(d.Last)}.{Transliterate(d.First)[0]}@osvita-plus.edu.ua"
            }));
        }
        return result;
    }

    private static Dictionary<string, Subject> AddSubjects(SchoolDatabase db, List<Teacher> t)
    {
        var data = new (string Name, int Hours, ControlForm Form, int Teacher, string Description)[]
        {
            ("Основи програмування", 150, ControlForm.Exam, 0, "Алгоритмізація та базові конструкції мови C#"),
            ("Об'єктно-орієнтоване програмування", 120, ControlForm.Exam, 0, "Класи, інкапсуляція, наслідування, поліморфізм"),
            ("Вища математика", 150, ControlForm.Exam, 1, "Лінійна алгебра, математичний аналіз"),
            ("Дискретна математика", 90, ControlForm.Credit, 1, "Множини, графи, комбінаторика, логіка"),
            ("Англійська мова (професійна)", 90, ControlForm.Credit, 2, "Англійська мова за професійним спрямуванням"),
            ("Бази даних", 120, ControlForm.Exam, 3, "Реляційна модель, SQL, проєктування БД"),
            ("Композиція та колористика", 120, ControlForm.Exam, 4, "Основи композиції та теорія кольору"),
            ("Комп'ютерна графіка", 90, ControlForm.Credit, 4, "Растрова та векторна графіка"),
            ("Мікроекономіка", 90, ControlForm.Exam, 5, "Поведінка споживачів і фірм на ринку"),
            ("Бухгалтерський облік", 120, ControlForm.Exam, 6, "Облік активів, зобов'язань та капіталу"),
            ("Комп'ютерні мережі", 90, ControlForm.Credit, 7, "Моделі OSI/TCP-IP, адресація, маршрутизація"),
            ("Українська мова за професійним спрямуванням", 60, ControlForm.Credit, 8, "Ділова документація та культура мовлення"),
            ("Веб-технології", 120, ControlForm.Exam, 9, "HTML, CSS, JavaScript, основи веб-дизайну")
        };

        return data.ToDictionary(d => d.Name, d => db.Subjects.Add(new Subject
        {
            Name = d.Name,
            Hours = d.Hours,
            ControlForm = d.Form,
            TeacherId = t[d.Teacher].Id,
            Description = d.Description
        }));
    }

    private static List<Group> AddGroups(SchoolDatabase db, List<Teacher> t, int ay)
    {
        string Yy(int year) => (year % 100).ToString("D2");
        var data = new (string Name, string Specialty, int Year, decimal Fee, int Curator)[]
        {
            ($"ПІ-{Yy(ay - 2)}", "Інженерія програмного забезпечення", ay - 2, 6800, 0),
            ($"ПІ-{Yy(ay - 1)}", "Інженерія програмного забезпечення", ay - 1, 6800, 3),
            ($"КН-{Yy(ay)}", "Комп'ютерні науки", ay, 6500, 9),
            ($"ГД-{Yy(ay - 1)}", "Графічний дизайн", ay - 1, 5900, 4),
            ($"ОО-{Yy(ay - 1)}", "Облік і оподаткування", ay - 1, 5200, 6)
        };

        return data.Select(d => db.Groups.Add(new Group
        {
            Name = d.Name,
            Specialty = d.Specialty,
            StartYear = d.Year,
            MonthlyFee = d.Fee,
            Capacity = 20,
            CuratorId = t[d.Curator].Id
        })).ToList();
    }

    private static void AddSchedule(SchoolDatabase db, Dictionary<Group, string[]> curriculum,
        Dictionary<string, Subject> subjects, Random rnd)
    {
        var schedule = new ScheduleService(db);
        var computerRooms = new[] { "301", "302", "305", "307" };
        var regularRooms = new[] { "101", "104", "112", "205", "210" };
        var computerSubjects = new HashSet<string>
        {
            "Основи програмування", "Об'єктно-орієнтоване програмування", "Бази даних",
            "Комп'ютерна графіка", "Комп'ютерні мережі", "Веб-технології"
        };

        foreach (var (group, names) in curriculum)
        {
            foreach (var name in names)
            {
                var subject = subjects[name];
                foreach (var type in new[] { LessonType.Lecture, computerSubjects.Contains(name) ? LessonType.Lab : LessonType.Practice })
                {
                    var rooms = type == LessonType.Lab ? computerRooms : regularRooms;
                    PlaceLesson(schedule, new Lesson
                    {
                        GroupId = group.Id,
                        SubjectId = subject.Id,
                        TeacherId = subject.TeacherId!.Value,
                        Type = type
                    }, rooms, rnd);
                }
            }
        }
    }

    private static void PlaceLesson(ScheduleService schedule, Lesson lesson, string[] rooms, Random rnd)
    {
        var days = LessonTimes.StudyDays.Take(5).OrderBy(_ => rnd.Next()).ToList();
        foreach (var number in new[] { 1, 2, 3, 4 })
        foreach (var day in days)
        foreach (var room in rooms.OrderBy(_ => rnd.Next()))
        {
            lesson.Day = day;
            lesson.Number = number;
            lesson.Room = room;
            if (schedule.FindConflicts(lesson).Count == 0)
            {
                schedule.Save(lesson);
                return;
            }
        }
    }

    private static Student CreateStudent(Group group, Random rnd, HashSet<string> usedNames, ref int contractNo, DateTime today)
    {
        string lastName, firstName, middleName, fatherName;
        bool male;
        do
        {
            male = rnd.Next(2) == 0;
            lastName = LastNames[rnd.Next(LastNames.Length)];
            firstName = male ? MaleNames[rnd.Next(MaleNames.Length)] : FemaleNames[rnd.Next(FemaleNames.Length)];
            var p = Patronymics[rnd.Next(Patronymics.Length)];
            middleName = male ? p.Male : p.Female;
            fatherName = p.Father;
        } while (!usedNames.Add($"{lastName} {firstName} {middleName}"));

        var enrollment = new DateTime(group.StartYear, 9, 1);
        if (rnd.NextDouble() < 0.1) enrollment = enrollment.AddMonths(1).AddDays(rnd.Next(0, 14));
        if (enrollment > today) enrollment = today;

        var discountRoll = rnd.NextDouble();
        var student = new Student
        {
            LastName = lastName,
            FirstName = firstName,
            MiddleName = middleName,
            BirthDate = new DateTime(group.StartYear - 16 - rnd.Next(0, 2), rnd.Next(1, 13), rnd.Next(1, 29)),
            Phone = RandomPhone(rnd),
            Email = $"{Transliterate(lastName)}.{Transliterate(firstName)}{rnd.Next(10, 99)}@gmail.com",
            GroupId = group.Id,
            ContractNumber = $"Д-{group.StartYear}-{contractNo++:D3}",
            EnrollmentDate = enrollment,
            DiscountPercent = discountRoll < 0.08 ? 25 : discountRoll < 0.2 ? 10 : 0,
            ParentName = $"{lastName} {fatherName}",
            ParentPhone = RandomPhone(rnd)
        };

        var statusRoll = rnd.NextDouble();
        var monthsStudied = (today - enrollment).TotalDays / 30;
        if (monthsStudied > 8 && statusRoll < 0.06)
        {
            student.Status = StudentStatus.Expelled;
            student.EndDate = enrollment.AddMonths(rnd.Next(4, (int)monthsStudied));
        }
        else if (monthsStudied > 3 && statusRoll < 0.1)
        {
            student.Status = StudentStatus.AcademicLeave;
        }

        return student;
    }

    private static void AddGrades(SchoolDatabase db, Student student, IEnumerable<Subject> subjects, Random rnd,
        DateTime today, int academicYear)
    {
        var ability = Math.Clamp(62 + rnd.Next(0, 35), 50, 96);
        var lastDate = student.EndDate ?? today.AddDays(-1);
        // Поточні оцінки — за останні ~3 місяці навчання.
        var recent = today.AddDays(-100);
        var from = recent > student.EnrollmentDate ? recent : student.EnrollmentDate.AddDays(7);
        if (from >= lastDate) return;

        var span = (int)(lastDate - from).TotalDays;
        foreach (var subject in subjects)
        {
            var types = new[] { GradeType.Practice, GradeType.Lab, GradeType.Module };
            var count = rnd.Next(2, 4);
            for (var i = 0; i < count; i++)
            {
                db.Grades.Add(new Grade
                {
                    StudentId = student.Id,
                    SubjectId = subject.Id,
                    Score = Math.Clamp(ability + rnd.Next(-15, 12), 25, 100),
                    Type = types[i % types.Length],
                    Date = from.AddDays(rnd.Next(0, Math.Max(1, span)))
                });
            }
        }

        // Підсумковий контроль за попередній семестр для студентів старших груп.
        var sessionDate = new DateTime(academicYear, 1, 20);
        if (student.EnrollmentDate < sessionDate.AddMonths(-3) && sessionDate < lastDate)
        {
            foreach (var subject in subjects.Take(3))
            {
                db.Grades.Add(new Grade
                {
                    StudentId = student.Id,
                    SubjectId = subject.Id,
                    Score = Math.Clamp(ability + rnd.Next(-10, 10), 35, 100),
                    Type = subject.ControlForm == ControlForm.Exam ? GradeType.Exam : GradeType.Credit,
                    Date = sessionDate.AddDays(rnd.Next(0, 10)),
                    Comment = "Сесія"
                });
            }
        }
    }

    private static IEnumerable<Payment> CreatePayments(Student student, decimal fee, Random rnd, DateTime today)
    {
        var months = new List<DateTime>();
        var end = student.EndDate ?? today;
        for (var m = new DateTime(student.EnrollmentDate.Year, student.EnrollmentDate.Month, 1); m <= end; m = m.AddMonths(1))
            if (FinanceService.IsBillableMonth(m.Month)) months.Add(m);

        // Поведінка платника: 70% — сумлінні, 20% — пропускають останні 1–2 місяці, 10% — боржники.
        var roll = rnd.NextDouble();
        var skipLast = roll < 0.7 ? 0 : roll < 0.9 ? rnd.Next(1, 3) : rnd.Next(3, 5);
        if (student.Status == StudentStatus.Expelled) skipLast = Math.Max(skipLast, 2);

        var paidMonths = months.Take(Math.Max(0, months.Count - skipLast)).ToList();
        var method = (PaymentMethod)rnd.Next(0, 3);

        for (var i = 0; i < paidMonths.Count; i++)
        {
            var date = paidMonths[i].AddDays(rnd.Next(0, 10));
            if (date > today) date = today;

            // Деякі сумлінні платники вносять одразу за два місяці.
            var monthsCovered = skipLast == 0 && i + 1 < paidMonths.Count && rnd.NextDouble() < 0.15 ? 2 : 1;
            yield return new Payment
            {
                StudentId = student.Id,
                Amount = fee * monthsCovered,
                Date = date,
                Method = rnd.NextDouble() < 0.8 ? method : (PaymentMethod)rnd.Next(0, 3),
                Purpose = monthsCovered == 2
                    ? $"Оплата за навчання ({MonthName(paidMonths[i])}, {MonthName(paidMonths[i + 1])})"
                    : $"Оплата за навчання ({MonthName(paidMonths[i])})",
                ReceiptNumber = "—"
            };
            i += monthsCovered - 1;
        }
    }

    private static string MonthName(DateTime d)
    {
        string[] names = { "січень", "лютий", "березень", "квітень", "травень", "червень", "липень", "серпень", "вересень", "жовтень", "листопад", "грудень" };
        return $"{names[d.Month - 1]} {d.Year}";
    }

    private static string RandomPhone(Random rnd) =>
        $"+38{PhonePrefixes[rnd.Next(PhonePrefixes.Length)]}{rnd.Next(1000000, 9999999)}";

    /// <summary>Транслітерація українських літер латиницею (для e-mail).</summary>
    public static string Transliterate(string text)
    {
        var map = new Dictionary<char, string>
        {
            ['а'] = "a", ['б'] = "b", ['в'] = "v", ['г'] = "h", ['ґ'] = "g", ['д'] = "d", ['е'] = "e", ['є'] = "ie",
            ['ж'] = "zh", ['з'] = "z", ['и'] = "y", ['і'] = "i", ['ї'] = "i", ['й'] = "i", ['к'] = "k", ['л'] = "l",
            ['м'] = "m", ['н'] = "n", ['о'] = "o", ['п'] = "p", ['р'] = "r", ['с'] = "s", ['т'] = "t", ['у'] = "u",
            ['ф'] = "f", ['х'] = "kh", ['ц'] = "ts", ['ч'] = "ch", ['ш'] = "sh", ['щ'] = "shch", ['ь'] = "", ['ю'] = "iu",
            ['я'] = "ia", ['\''] = "", ['ʼ'] = "", ['’'] = "", ['-'] = "-", [' '] = ""
        };
        return string.Concat(text.ToLowerInvariant().Select(c => map.TryGetValue(c, out var s) ? s : c.ToString()));
    }
}
