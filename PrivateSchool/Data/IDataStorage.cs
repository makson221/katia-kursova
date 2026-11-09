namespace PrivateSchool.Data
{
    /// <summary>
    /// Абстракція постійного сховища даних. Дозволяє замінити спосіб зберігання
    /// (XML, JSON, база даних) без зміни решти програми.
    /// </summary>
    public interface IDataStorage
    {
        /// <summary>Опис місця зберігання (наприклад, шлях до файлу).</summary>
        string Location { get; }

        SchoolData Load();

        void Save(SchoolData data);
    }
}
