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
		void Create(T entity);
		void Delete(T entity);
		IEnumerable<T> GetAll();
		void Update(T entity);
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


		public virtual void Create(T entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}

			_dbset.Add(entity);
			_context.SaveChangesAsync();
		}


		public virtual void Update(T entity)
		{
			if (entity == null) throw new ArgumentNullException("entity");
			_context.Entry(entity).State = EntityState.Modified;
			_context.SaveChangesAsync();
		}

		public virtual void Delete(T entity)
		{
			if (entity == null) throw new ArgumentNullException("entity");
			_dbset.Remove(entity);
			_context.SaveChangesAsync();
		}

		public virtual IEnumerable<T> GetAll()
		{
			return _dbset.AsEnumerable<T>();
		}
	}
}