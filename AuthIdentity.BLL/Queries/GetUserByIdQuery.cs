using AuthIdentity.DTO.Response;
using CORE.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthIdentity.BLL.Queries
{
    public class GetUserByIdQuery : IRequest<SingleResponse<GetUserDetailsResp>>
    {
        public Guid UserGuid { get; set; }
    }
}
