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
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, SingleResponse<GetUserDetailsResp>>
    {
        private readonly IRepositoryAsync<TblUser> _repository;
        private readonly IMapper _mapper;
        public GetUserByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = unitOfWork.GetRepositoryAsync<TblUser>();
            _mapper = mapper;
        }

        public async Task<SingleResponse<GetUserDetailsResp>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken = default)
        {
            var response = new SingleResponse<GetUserDetailsResp>(null);
            var result = await _repository.SingleOrDefaultAsync(x => x.UserGuid == request.UserGuid);
            if (result != null)
            {
                var res = _mapper.Map<GetUserDetailsResp>(result);
                response = new SingleResponse<GetUserDetailsResp>(res);
            }
            return response;
        }
    }
}