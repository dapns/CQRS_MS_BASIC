using System;
using System.Collections.Generic;

namespace AuthIdentity.DAL.Context
{
    public partial class TblUserRoleMap
    {
        public int UserRoleMap { get; set; }
        public long UserId { get; set; }
        public int RoleId { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public byte[]? IpAddress { get; set; }

        public virtual TblRole Role { get; set; } = null!;
        public virtual TblUser User { get; set; } = null!;
    }
}
