using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using PrivateSchool.Core.Interfaces;

namespace PrivateSchool.Core.Data;

/// <summary>
/// Сховище даних у JSON-файлі. Запис виконується атомарно через тимчасовий файл,
/// а перед перезаписом створюється резервна копія.
/// </summary>
public class JsonDataStorage : IDataStorage
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        Converters = { new JsonStringEnumConverter() }
    };

    public string FilePath { get; }

    public JsonDataStorage(string filePath)
    {
        FilePath = filePath;
    }

    public bool Exists => File.Exists(FilePath);

    public SchoolData Load()
    {
        if (!Exists) return new SchoolData();

        var json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<SchoolData>(json, Options) ?? new SchoolData();
    }

    public void Save(SchoolData data)
    {
        var directory = Path.GetDirectoryName(Path.GetFullPath(FilePath));
        if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);

        var tempPath = FilePath + ".tmp";
        File.WriteAllText(tempPath, JsonSerializer.Serialize(data, Options));

        if (File.Exists(FilePath))
            File.Copy(FilePath, FilePath + ".bak", overwrite: true);

        File.Move(tempPath, FilePath, overwrite: true);
    }
}
