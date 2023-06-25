using System;
using System.Collections.Generic;

namespace AuthIdentity.DAL.Context
{
    public partial class TblUser
    {
        public TblUser()
        {
            TblUserRoleMaps = new HashSet<TblUserRoleMap>();
        }

        public long UserId { get; set; }
        public Guid UserGuid { get; set; }
        public string UserLoginId { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? UserKey { get; set; }
        public string? Salt { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public byte[]? IpAddress { get; set; }

        public virtual ICollection<TblUserRoleMap> TblUserRoleMaps { get; set; }
    }
}
