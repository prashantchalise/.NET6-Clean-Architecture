/*
 Service For Bill
Created by: Prashant
Created On: 30/07/2022
*/
using Microsoft.EntityFrameworkCore;
using PROJBP.Model;

namespace PROJBP.Service
{
	public interface IBillService : IEntityService<Bill>
	{
		Task<Bill> GetBillByIdAsync(int billid); 
	}

	public class BillService : EntityService<Bill>, IBillService
	{

		new IContext _context;
		public BillService(IContext context) : base(context)
		{
			_context = context;
			_dbset = _context.Set<Bill>();
		}

		/// <summary>
		/// Get Bill By Id :: Don't forget to add the DBSet to BabyAZContext
		/// </summary>

		public async Task<Bill> GetBillByIdAsync(int billid)
		{
			return await _dbset.FirstOrDefaultAsync(x => x.BillId == billid);
		}
	}
}
