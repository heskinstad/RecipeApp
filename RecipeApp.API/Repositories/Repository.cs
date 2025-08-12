using Microsoft.EntityFrameworkCore;
using RecipeApp.API.Data;
using RecipeApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp.API.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private RecipeContext _db;
        private DbSet<T> _table = null!;

        public Repository(RecipeContext db)
        {
            _db = db;
            _table = _db.Set<T>();
        }

        public async Task<IEnumerable<T>> Get()
        {
            return _table.ToList();
        }

        public async Task<T> Insert(T entity)
        {
            _table.Add(entity);
            _db.SaveChanges();
            return entity;
        }

        public async Task<T> Update(T entity)
        {
            _table.Attach(entity);
            _db.Entry(entity).State = EntityState.Modified;
            _db.SaveChanges();
            return entity;
        }

        public async Task<T> Delete(object id)
        {
            T entity = _table.Find(id);
            _table.Remove(entity);
            _db.SaveChanges();
            return entity;
        }

        public async Task<T> GetById(object id)
        {
            return _table.Find(id);
        }

        public IQueryable<T> GetQueryable(Expression<Func<T, bool>>? predicate = null)
        {
            IQueryable<T> query = _table;

            if (predicate != null)
                query = query.Where(predicate);

            return query;
        }
    }
}
