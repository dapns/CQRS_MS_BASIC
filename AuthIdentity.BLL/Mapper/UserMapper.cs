using AuthIdentity.DAL.Context;
using AuthIdentity.DTO.Response;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AuthIdentity.BLL.Mapper
{
    public class UserMapper:Profile
    {
        public UserMapper() 
        {
            this.CreateMap<TblUser, GetUserDetailsResp>();
        }
    }
}
