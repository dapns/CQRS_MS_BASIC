using AuthIdentity.BLL.Helper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthIdentity.BLL.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        // Dependency Injection
        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            //Reading the AuthHeader which is signed with JWT
            string authHeader = context.Request.Headers["Authorization"];

            if (authHeader != null)
            {

                var token = authHeader.Split(" ").Last();
                var claim = new JwtSecurityTokenHandler().ReadJwtToken(token);

                var claimName = claim.Claims.FirstOrDefault(c => c.Type == "userhash").Value;

                //Reading the JWT middle part           
                //int startPoint = authHeader.IndexOf(".") + 1;
                //int endPoint = authHeader.LastIndexOf(".");

                //var tokenString = authHeader
                //    .Substring(startPoint, endPoint - startPoint).Split(".");
                //var token = tokenString[0].ToString() + "==";

                //var credentialString = Encoding.UTF8
                //    .GetString(Convert.FromBase64String(token));

                //// Splitting the data from Jwt
                //var credentials = credentialString.Split(new char[] { ':', ',' });

                //// Trim this Username and UserRole.
                //var userRule = credentials[5].Replace("\"", "");
                //var userName = credentials[3].Replace("\"", "");

                var tokenUser = TokenHelper.GetUnHashUser(claimName);

                // Identity Principal
                var claims = new[]
                {
                    new Claim("name", tokenUser.UserGuid.ToString()),
                    new Claim(ClaimTypes.Role, tokenUser.UserId.ToString()),
                };
                var identity = new ClaimsIdentity(claims, "basic");
                context.User = new ClaimsPrincipal(identity);
            }
            //Pass to the next middleware
            await _next(context);
        }
    }
}
