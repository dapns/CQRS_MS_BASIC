using CORE.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthIdentity.BLL.Queries
{
    public class UserLoginQuery: IRequest<SingleResponse<string>>
    {
        public string UserLoginId { get; set; }
    }
}
