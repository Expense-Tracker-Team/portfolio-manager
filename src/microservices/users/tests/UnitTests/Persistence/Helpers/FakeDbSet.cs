namespace UnitTests.Persistence.Helpers
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Linq.Expressions;

    public class FakeDbSet<T> : DbSet<T> where T : class
    {
        private readonly IQueryable _query;

        public FakeDbSet()
        {
            Local = new ObservableCollection<T>();
            _query = Local.AsQueryable();
        }

        public T Add(T item)
        {
            Local.Add(item);
            return item;
        }

        public T Remove(T item)
        {
            Local.Remove(item);
            return item;
        }

        public T Attach(T item)
        {
            Local.Add(item);
            return item;
        }

        public T Detach(T item)
        {
            Local.Remove(item);
            return item;
        }

        public T Create() => Activator.CreateInstance<T>();

        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
            => Activator.CreateInstance<TDerivedEntity>();

        public ObservableCollection<T> Local { get; }

        public Type ElementType => _query.ElementType;

        public Expression Expression => _query.Expression;

        public IQueryProvider Provider => _query.Provider;

        public IEnumerator<T> GetEnumerator() => Local.GetEnumerator();
    }
}
