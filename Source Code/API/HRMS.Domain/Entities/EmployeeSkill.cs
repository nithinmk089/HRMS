using System;

namespace HRMS.Domain.Entities
{
    public class EmployeeSkill
    {
        public long EmployeeSkillID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long SkillID { get; set; }
        public string SkillLevel { get; set; } = "Beginner";
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