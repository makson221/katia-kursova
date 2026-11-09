using PrivateSchool.Core.Data;
using PrivateSchool.Core.Models;
using PrivateSchool.Core.Reports;
using PrivateSchool.Core.Services;
using PrivateSchool.WinForms.UI.Controls;
using PrivateSchool.WinForms.UI.Dialogs;

namespace PrivateSchool.WinForms.UI.Views;

public sealed class UsersView : CrudView<User>
{
    public UsersView(SchoolSystem app) : base(app) { }

    public override string Title => "Користувачі";
    public override string Subtitle => "Облікові записи та ролі доступу до системи";

    protected override Repository<User> Repository => App.Db.Users;
    protected override string EntityName => "користувача";
    protected override bool CanModify => App.Auth.Can(Permission.ManageUsers);

    protected override void ConfigureColumns(StyledGrid grid)
    {
        grid.AddColumn("Логін", 90);
        grid.AddColumn("Ім'я", 200);
        grid.AddColumn("Роль", 130);
        grid.AddColumn("Пов'язаний викладач", 180);
        grid.AddColumn("Активний", 70, ColumnAlign.Center);
        grid.AddColumn("Останній вхід", 120, ColumnAlign.Center, "dd.MM.yyyy HH:mm");
    }

    protected override object?[] ToRow(User u) => new object?[]
    {
        u.Login,
        u.DisplayName,
        u.Role.ToDisplay(),
        u.TeacherId is int t ? App.Db.Teachers.GetById(t)?.FullName : "—",
        u.IsActive ? "Так" : "Ні",
        u.LastLogin
    };

    protected override Color? GetRowColor(User u) => u.IsActive ? null : Theme.TextMuted;

    protected override string GetSearchText(User u) => $"{u.Login} {u.DisplayName} {u.Role.ToDisplay()}";

    protected override User CreateNew() => new() { Role = UserRole.Manager };

    /// <summary>Збереження користувачів виконує сам діалог через AuthService (хешування пароля).</summary>
    protected override Form CreateDialog(User item, bool isNew, Action<User> save) => new UserDialog(App, item, isNew);
}
