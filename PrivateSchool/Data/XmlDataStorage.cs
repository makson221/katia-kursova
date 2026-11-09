using System;
using System.IO;
using System.Xml.Serialization;

namespace PrivateSchool.Data
{
    /// <summary>
    /// Збереження даних у XML-файл за допомогою XmlSerializer.
    /// Перед перезаписом створюється резервна копія файлу.
    /// </summary>
    public class XmlDataStorage : IDataStorage
    {
        private readonly string _filePath;
        private readonly XmlSerializer _serializer = new XmlSerializer(typeof(SchoolData));

        public XmlDataStorage(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Не вказано шлях до файлу даних.", "filePath");
            _filePath = filePath;
        }

        public string Location
        {
            get { return _filePath; }
        }

        public SchoolData Load()
        {
            if (!File.Exists(_filePath))
                return new SchoolData();

            try
            {
                using (FileStream stream = File.OpenRead(_filePath))
                {
                    return (SchoolData)_serializer.Deserialize(stream) ?? new SchoolData();
                }
            }
            catch (InvalidOperationException ex)
            {
                throw new IOException("Файл даних пошкоджено або має невірний формат: " + _filePath, ex);
            }
        }

        public void Save(SchoolData data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            string directory = Path.GetDirectoryName(Path.GetFullPath(_filePath));
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            // Спершу пишемо у тимчасовий файл, щоб не пошкодити дані при збої
            string tempPath = _filePath + ".tmp";
            using (FileStream stream = File.Create(tempPath))
            {
                _serializer.Serialize(stream, data);
            }

            if (File.Exists(_filePath))
                File.Copy(_filePath, _filePath + ".bak", true);
            File.Copy(tempPath, _filePath, true);
            File.Delete(tempPath);
        }
    }
}
