using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DibaEShop.DataAccess.Repository.IRepository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> filter, string? includeProperties = null , bool tracked = true);
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null, string? includeProperties = null);
        void Add(TEntity entity);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entity);

    }
}
