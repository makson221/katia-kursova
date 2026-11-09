using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using PrivateSchool.Data;
using PrivateSchool.Forms;
using PrivateSchool.Models;
using PrivateSchool.Services;

namespace PrivateSchool
{
    internal static class Program
    {
        /// <summary>
        /// Точка входу в програму.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("uk-UA");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("uk-UA");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (s, e) => ShowUnexpectedError(e.Exception);

            string dataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "school.xml");

            SchoolService service;
            try
            {
                service = new SchoolService(new XmlDataStorage(dataFile));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не вдалося завантажити дані:\n" + ex.Message +
                                "\n\nПеревірте файл " + dataFile + " або видаліть його, щоб створити нову базу.",
                    "Помилка запуску", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AuthService auth = new AuthService(service);

            // Цикл дозволяє змінити користувача без перезапуску програми
            while (true)
            {
                User user;
                using (LoginForm login = new LoginForm(auth, service.SchoolName))
                {
                    if (login.ShowDialog() != DialogResult.OK)
                        return;
                    user = login.AuthenticatedUser;
                }

                using (MainForm main = new MainForm(service, user))
                {
                    Application.Run(main);
                    if (!main.LogoutRequested)
                        return;
                }
            }
        }

        private static void ShowUnexpectedError(Exception ex)
        {
            if (ex is ValidationException)
            {
                MessageBox.Show(ex.Message, "Перевірка даних", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            MessageBox.Show("Сталася непередбачена помилка:\n" + ex.Message, "Помилка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
