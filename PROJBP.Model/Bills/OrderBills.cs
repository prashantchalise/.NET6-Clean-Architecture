/* 
Model for [dbo].[Bill] 
Created by: Prashant 
Created On: 30/07/2022 
 */
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROJBP.Model
{
	[Table("TBL_Bill")]
	public class Bill : AuditableEntity<long>
	{

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int BillId { get; set; }


		[Required]
		[MaxLength(50)]
		[Display(Name = "BillNumber")]
		public string BillNumber { get; set; }


		[Display(Name = "BilledDateUTC")]
		public DateTime BilledDateUTC { get; set; }


		[MaxLength(50)]
		[Display(Name = "CustomerName")]
		public string CustomerName { get; set; }


		[MaxLength(2000)]
		[Display(Name = "CustomerAddress")]
		public string CustomerAddress { get; set; }


		[MaxLength(50)]
		[Display(Name = "VATNo")]
		public string VATNo { get; set; }


		[MaxLength(50)]
		[Display(Name = "BillStatus")]
		public string BillStatus { get; set; }

	}

	/* Bill View Model */
	public class BillViewModel
	{
		public Int64 RowNumber { get; set; }
		public int BillId { get; set; }
		public string BillNumber { get; set; }
		public DateTime BilledDateUTC { get; set; }
		public string CustomerName { get; set; }
		public string CustomerAddress { get; set; }
		public string VATNo { get; set; }
		public string BillStatus { get; set; }

		public int TotalCount { get; set; }
	}

	/* Bill View Model (Input) */
	public class BillViewModel_Input
	{
		public int? BillId { get; set; }
		public string BillNumber { get; set; }
		public int? PageNumber { get; set; }
		public int? PageSize { get; set; }
		public int? ShowAll { get; set; }
	}
}
