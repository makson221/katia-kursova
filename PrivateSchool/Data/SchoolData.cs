using System;
using System.Collections.Generic;
using PrivateSchool.Models;

namespace PrivateSchool.Data
{
    /// <summary>
    /// Контейнер усіх даних закладу, який повністю серіалізується у файл.
    /// </summary>
    [Serializable]
    public class SchoolData
    {
        public SchoolData()
        {
            SchoolName = "Приватний навчальний заклад «Знання+»";
            Students = new List<Student>();
            Teachers = new List<Teacher>();
            Courses = new List<Course>();
            Groups = new List<Group>();
            Lessons = new List<Lesson>();
            Payments = new List<Payment>();
            Grades = new List<Grade>();
            Users = new List<User>();
        }

        public string SchoolName { get; set; }

        public List<Student> Students { get; set; }
        public List<Teacher> Teachers { get; set; }
        public List<Course> Courses { get; set; }
        public List<Group> Groups { get; set; }
        public List<Lesson> Lessons { get; set; }
        public List<Payment> Payments { get; set; }
        public List<Grade> Grades { get; set; }
        public List<User> Users { get; set; }

        public bool IsEmpty
        {
            get { return Users.Count == 0; }
        }
    }
}
