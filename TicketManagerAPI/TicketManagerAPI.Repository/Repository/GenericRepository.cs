using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TicketManagerAPI.Repository.Utils;

namespace TicketManagerAPI.Repository.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> where = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");

        Task<T> GetByIdAsync(Expression<Func<T, bool>> where = null,
            string includeProperties = "");

        Task<bool> CreateAsync(T entity);

        Task<bool> UpdateAsync(T entity);

        Task<bool> DeleteAsync(T entity);
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenericRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> where = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = _unitOfWork.Context.Set<T>();

            if (!LogicValidations.ValidateIfDataIsNull(where))
            {
                query = query.Where(where);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (!LogicValidations.ValidateIfDataIsNull(orderBy))
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> where = null,
            string includeProperties = "")
        {
            IQueryable<T> query = _unitOfWork.Context.Set<T>();

            if (!LogicValidations.ValidateIfDataIsNull(where))
            {
                query = query.Where(where);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> CreateAsync(T entity)
        {
            bool requestResult = true;
            try
            {
                await _unitOfWork.Context.Set<T>().AddAsync(entity);
                await _unitOfWork.Save();
            }
            catch
            {
                requestResult = false;
            }
            return await Task.FromResult(requestResult);
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            bool requestResult = true;
            try
            {
                _unitOfWork.Context.Set<T>().Update(entity);
                await _unitOfWork.Save();
            }
            catch
            {
                requestResult = false;
            }
            return await Task.FromResult(requestResult);
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            bool requestResult = true;
            try
            {
                _unitOfWork.Context.Set<T>().Remove(entity);
                await _unitOfWork.Save();
            }
            catch
            {
                requestResult = false;
            }
            return await Task.FromResult(requestResult);
        }
    }
}