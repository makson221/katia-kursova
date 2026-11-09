using System;
using System.Collections.Generic;
using PrivateSchool.Models;

namespace PrivateSchool.Data
{
    /// <summary>
    /// Узагальнений інтерфейс сховища сутностей (шаблон «Репозиторій»).
    /// </summary>
    public interface IRepository<T> where T : EntityBase
    {
        event EventHandler Changed;

        int Count { get; }

        IReadOnlyList<T> GetAll();

        T GetById(int id);

        IEnumerable<T> Find(Func<T, bool> predicate);

        IEnumerable<T> Search(string query);

        void Add(T item);

        void Update(T item);

        bool Remove(int id);

        int RemoveWhere(Func<T, bool> predicate);
    }
}
