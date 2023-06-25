using AuthIdentity.BLL.Helper;
using AuthIdentity.BLL.Queries;
using AuthIdentity.DTO.Response;
using Core.Helper;
using CORE.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthIdentity.BLL.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettingsHelper _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettingsHelper> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IMediator mediator)
        {
            var token = UserHelper.GetHeaderToken(context);

            if (token != null)
                AttachUserToContext(context, mediator, token);

            await _next(context);
        }

        private void AttachUserToContext(HttpContext context, IMediator mediator, string token)
        {
            try
            {
                //if (token == null)
                //    return null;

                var tokenHandler = new JwtSecurityTokenHandler();
                var claim = new JwtSecurityTokenHandler().ReadJwtToken(token);
               
                var claimName = claim.Claims.FirstOrDefault(c => c.Type == "userhash").Value; 
                var user = new SingleResponse<GetUserDetailsResp>(null);
                if (claimName != null)
                {
                    var tokenUser = TokenHelper.GetUnHashUser(claimName);
                    var input = new GetUserByIdQuery { UserGuid = tokenUser.UserGuid };
                    user = mediator.Send(input).GetAwaiter().GetResult();
                }

                if (user != null)
                {
                    var key = Encoding.ASCII.GetBytes(user.Data.UserKey);
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;

                    // attach user to context on successful jwt validation
                    context.Items["User"] = claimName;
                }
                else
                    context.Items["User"] = null;

            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}
