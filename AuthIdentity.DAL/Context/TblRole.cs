using System;
using System.Collections.Generic;

namespace AuthIdentity.DAL.Context
{
    public partial class TblRole
    {
        public TblRole()
        {
            TblUserRoleMaps = new HashSet<TblUserRoleMap>();
        }

        public int RoleId { get; set; }
        public Guid RoleGuid { get; set; }
        public string RoleName { get; set; } = null!;
        public string? RoleDesc { get; set; }
        public int? Priority { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public byte[]? IpAddress { get; set; }

        public virtual ICollection<TblUserRoleMap> TblUserRoleMaps { get; set; }
    }
}
