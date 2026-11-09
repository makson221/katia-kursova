using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace PrivateSchool.Utils
{
    /// <summary>
    /// Допоміжні методи для відображення значень перелічень українською мовою.
    /// Назва береться з атрибута [Description].
    /// </summary>
    public static class EnumHelper
    {
        public static string ToText(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            if (field == null)
                return value.ToString();
            DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute != null ? attribute.Description : value.ToString();
        }

        public static IEnumerable<ComboItem> GetItems<T>() where T : struct
        {
            return Enum.GetValues(typeof(T)).Cast<Enum>().Select(v => new ComboItem(v, ToText(v)));
        }
    }

    /// <summary>
    /// Українські назви днів тижня.
    /// </summary>
    public static class DayNames
    {
        private static readonly string[] Names =
        {
            "Неділя", "Понеділок", "Вівторок", "Середа", "Четвер", "П'ятниця", "Субота"
        };

        /// <summary>Робочий порядок днів (з понеділка).</summary>
        public static readonly DayOfWeek[] WeekOrder =
        {
            DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday,
            DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday
        };

        public static string ToText(DayOfWeek day)
        {
            return Names[(int)day];
        }

        /// <summary>Порядковий номер дня, де понеділок = 0.</summary>
        public static int OrderOf(DayOfWeek day)
        {
            return ((int)day + 6) % 7;
        }

        public static IEnumerable<ComboItem> GetItems()
        {
            return WeekOrder.Select(d => new ComboItem(d, ToText(d)));
        }
    }
}
