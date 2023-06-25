using AuthIdentity.BLL.Helper;
using AuthIdentity.BLL.Queries;
using AuthIdentity.DAL.Context;
using AuthIdentity.DTO.Response;
using AutoMapper;
using CORE.Repository;
using CORE.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthIdentity.BLL.Handlers
{
    public class UserLoginHandler : IRequestHandler<UserLoginQuery, SingleResponse<string>>
    {
        private readonly IRepositoryAsync<TblUser> _repository;
        private readonly IMapper _mapper;
        public UserLoginHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = unitOfWork.GetRepositoryAsync<TblUser>();
            _mapper = mapper;
        }

        public async Task<SingleResponse<string>> Handle(UserLoginQuery request, CancellationToken cancellationToken = default)
        {
            var response = new SingleResponse<string>(null);
            var result = await _repository.SingleOrDefaultAsync(x => x.UserLoginId == request.UserLoginId);
            if (result != null)
            {
                var res = _mapper.Map<GetUserDetailsResp>(result);
                var token = TokenHelper.GenerateJwtToken(res);
                response = new SingleResponse<string>(token);
            }
            return response;
        }
    }
}
