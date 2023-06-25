using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthIdentity.DTO.Response
{
    public class GetUserDetailsResp
    {
        public long UserId { get; set; }
        public Guid UserGuid { get; set; }
        public string UserLoginId { get; set; } = null!;
        public string? UserKey { get; set; }
        public string? Salt { get; set; }
    }
}
