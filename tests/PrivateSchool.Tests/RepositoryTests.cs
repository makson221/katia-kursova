using PrivateSchool.Core.Data;
using PrivateSchool.Core.Exceptions;
using PrivateSchool.Core.Models;

namespace PrivateSchool.Tests;

public class RepositoryTests
{
    [Fact]
    public void Add_AssignsIncrementalIds_AndRaisesChanged()
    {
        var repo = new Repository<Subject>();
        var changes = 0;
        repo.Changed += (_, _) => changes++;

        var a = repo.Add(new Subject { Name = "A", Hours = 30 });
        var b = repo.Add(new Subject { Name = "B", Hours = 30 });

        Assert.Equal(1, a.Id);
        Assert.Equal(2, b.Id);
        Assert.Equal(2, changes);
    }

    [Fact]
    public void Add_InvalidEntity_Throws()
    {
        var repo = new Repository<Subject>();
        var ex = Assert.Throws<ValidationException>(() => repo.Add(new Subject { Name = "", Hours = 0 }));
        Assert.Equal(2, ex.Errors.Count);
        Assert.Equal(0, repo.Count);
    }

    [Fact]
    public void Update_ReplacesItem_RemoveDeletes()
    {
        var repo = new Repository<Subject>();
        var s = repo.Add(new Subject { Name = "A", Hours = 30 });

        var copy = (Subject)s.Clone();
        copy.Name = "B";
        repo.Update(copy);
        Assert.Equal("B", repo.GetById(s.Id)!.Name);

        Assert.True(repo.Remove(s.Id));
        Assert.Null(repo.GetById(s.Id));
        Assert.Throws<DomainException>(() => repo.Update(copy));
    }

    [Fact]
    public void JsonStorage_RoundTrip()
    {
        var path = Path.Combine(Path.GetTempPath(), $"school-{Guid.NewGuid():N}.json");
        try
        {
            var storage = new JsonDataStorage(path);
            var db = new SchoolDatabase(storage);
            var group = TestHelpers.AddGroup(db);
            TestHelpers.AddStudent(db, group);

            var loaded = new SchoolDatabase(new JsonDataStorage(path));
            loaded.Load();

            Assert.Equal(1, loaded.Students.Count);
            Assert.Equal("Петренко Іван Петрович", loaded.Students.GetAll()[0].FullName);
            Assert.Equal("ПІ-24", loaded.Groups.GetAll()[0].Name);

            // Нові записи продовжують нумерацію після завантаження.
            Assert.Equal(2, TestHelpers.AddGroup(loaded, name: "КН-24").Id);
        }
        finally
        {
            foreach (var f in new[] { path, path + ".bak", path + ".tmp" })
                if (File.Exists(f)) File.Delete(f);
        }
    }

    [Fact]
    public void RunInBatch_SavesOnce()
    {
        var storage = new InMemoryDataStorage();
        var db = new SchoolDatabase(storage);
        db.RunInBatch(() =>
        {
            TestHelpers.AddGroup(db, name: "A-1");
            TestHelpers.AddGroup(db, name: "A-2");
        });
        Assert.Equal(1, storage.SaveCount);
    }
}
