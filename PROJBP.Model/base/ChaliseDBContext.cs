using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace PROJBP.Model
{

    public interface IContext
    {
		DbSet<Bill> Bill { get; set; }

		DbSet<TEntity> Set<TEntity>() where TEntity : class;
		EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class; 
		Task<int> SaveChangesAsync();
	}
	 

	public class ChaliseDBContext : DbContext, IContext
	{
		#region Ctor
		public ChaliseDBContext(DbContextOptions<ChaliseDBContext> options)
		 : base(options)
		{
		}
		 
		#endregion

		public DbSet<Bill> Bill { get; set; }


		#region Methods
		public Task<int> SaveChangesAsync()
		{
			var modifiedEntries = ChangeTracker.Entries()
				.Where(x => x.Entity is IAuditableEntity
					&& (x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted));

			foreach (var entry in modifiedEntries)
			{
				IAuditableEntity entity = entry.Entity as IAuditableEntity;
				if (entity != null)
				{
					string identityName = "";
					if (Thread.CurrentPrincipal is not null)
					{
						identityName = Thread.CurrentPrincipal.Identity.Name;
					}
					
					
					DateTime now = DateTime.UtcNow;

					if (entry.State == EntityState.Added)
					{
						entity.CreatedBy = identityName;
						entity.CreatedDate = now;

						entity.UpdatedDate = null;
						entity.DeletedDate = null;

					}
					else if (entry.State == EntityState.Modified)
					{
						entity.UpdatedBy = identityName;
						entity.UpdatedDate = now;

						entity.DeletedDate = null;

					}
					else
					{
						entry.State = EntityState.Modified;

						entity.DeletedBy = identityName;
						entity.DeletedDate = now;
					}


				}
			}

			return base.SaveChangesAsync();
		}
 

        #endregion




    }
}
