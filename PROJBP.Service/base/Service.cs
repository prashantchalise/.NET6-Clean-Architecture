using PROJBP.Model;
using Microsoft.EntityFrameworkCore;

namespace PROJBP.Service
{
	public interface IService
	{
	}

	public interface IEntityService<T> : IService
	where T : BaseEntity
	{
		Task CreateAsync(T entity);
		Task DeleteAsync(T entity);
		Task<IList<T>> GetAllAsync();
		Task UpdateAsync(T entity);
	}

	public abstract class EntityService<T> : IEntityService<T> where T : BaseEntity
	{
		protected IContext _context;
		protected DbSet<T> _dbset;

		public EntityService(IContext context)
		{
			_context = context;
			_dbset = _context.Set<T>();  

		}


		public virtual async Task CreateAsync(T entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}

			await _dbset.AddAsync(entity);

			await _context.SaveChangesAsync();
		}


        public virtual async Task UpdateAsync(T entity)
		{
			if (entity == null) throw new ArgumentNullException("entity");
			_context.Entry(entity).State = EntityState.Modified;
			await _context.SaveChangesAsync();
		}

		public virtual async  Task DeleteAsync(T entity)
		{
			if (entity == null) throw new ArgumentNullException("entity");
			_dbset.Remove(entity);
			await _context.SaveChangesAsync();
		}

        public virtual async Task<IList<T>> GetAllAsync()
        {
			return await _dbset.ToListAsync<T>(); 
        }
    }
}