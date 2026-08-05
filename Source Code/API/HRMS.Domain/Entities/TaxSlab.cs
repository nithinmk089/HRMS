using System;

namespace HRMS.Domain.Entities
{
    public class TaxSlab
    {
        public long TaxSlabID { get; set; }
        public long TenantID { get; set; }
        public long TaxRegimeID { get; set; }
        public decimal IncomeFrom { get; set; }
        public decimal? IncomeTo { get; set; }
        public decimal TaxRate { get; set; }

        // Audit Columns
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}
