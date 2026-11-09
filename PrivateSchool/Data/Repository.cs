using System;
using System.Collections.Generic;
using System.Linq;
using PrivateSchool.Models;

namespace PrivateSchool.Data
{
    /// <summary>
    /// Узагальнена (generic) реалізація репозиторію у пам'яті.
    /// Працює з будь-яким типом, похідним від EntityBase.
    /// </summary>
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        private readonly List<T> _items;

        /// <summary>Подія, що виникає після будь-якої зміни даних.</summary>
        public event EventHandler Changed;

        public Repository(List<T> storage)
        {
            if (storage == null)
                throw new ArgumentNullException("storage");
            _items = storage;
        }

        public int Count
        {
            get { return _items.Count; }
        }

        public IReadOnlyList<T> GetAll()
        {
            return _items.AsReadOnly();
        }

        public T GetById(int id)
        {
            return _items.FirstOrDefault(i => i.Id == id);
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return _items.Where(predicate);
        }

        public IEnumerable<T> Search(string query)
        {
            return _items.Where(i => i.Matches(query));
        }

        public void Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            item.Validate();
            item.Id = NextId();
            _items.Add(item);
            OnChanged();
        }

        public void Update(T item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            int index = _items.FindIndex(i => i.Id == item.Id);
            if (index < 0)
                throw new ValidationException("Запис не знайдено. Можливо, його вже видалено.");
            item.Validate();
            _items[index] = item;
            OnChanged();
        }

        public bool Remove(int id)
        {
            int removed = _items.RemoveAll(i => i.Id == id);
            if (removed > 0)
                OnChanged();
            return removed > 0;
        }

        public int RemoveWhere(Func<T, bool> predicate)
        {
            int removed = _items.RemoveAll(i => predicate(i));
            if (removed > 0)
                OnChanged();
            return removed;
        }

        private int NextId()
        {
            return _items.Count == 0 ? 1 : _items.Max(i => i.Id) + 1;
        }

        protected virtual void OnChanged()
        {
            EventHandler handler = Changed;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}
