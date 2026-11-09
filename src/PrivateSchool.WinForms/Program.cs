using PrivateSchool.Core.Data;
using PrivateSchool.Core.Services;
using PrivateSchool.WinForms.UI;
using PrivateSchool.WinForms.UI.Forms;

namespace PrivateSchool.WinForms;

internal static class Program
{
    /// <summary>
    /// Точка входу: ініціалізація системи, цикл "вхід → головне вікно → вихід з облікового запису".
    /// </summary>
    [STAThread]
    private static void Main()
    {
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += (_, e) => Messages.Exception(e.Exception);

        var dataPath = Path.Combine(AppContext.BaseDirectory, "Data", "school.json");

        SchoolSystem app;
        try
        {
            app = new SchoolSystem(new JsonDataStorage(dataPath));
            if (app.Initialize())
                Messages.Info("Створено демонстраційну базу даних.\n\n" +
                              "Облікові записи для входу:\n" +
                              "  admin / admin123 — адміністратор\n" +
                              "  manager / manager123 — менеджер\n" +
                              "  teacher / teacher123 — викладач");
        }
        catch (Exception ex)
        {
            Messages.Error($"Не вдалося завантажити дані з файлу:\n{dataPath}\n\n{ex.Message}\n\n" +
                           "Перевірте файл або видаліть його, щоб створити нову демонстраційну базу " +
                           "(резервна копія зберігається поруч із розширенням .bak).");
            return;
        }

        while (true)
        {
            using (var login = new LoginForm(app))
            {
                if (login.ShowDialog() != DialogResult.OK) return;
            }

            bool logout;
            using (var main = new MainForm(app, dataPath))
            {
                Application.Run(main);
                logout = main.LogoutRequested;
            }

            app.Auth.Logout();
            if (!logout) return;
        }
    }
}
