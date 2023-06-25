using AuthIdentity.DTO.Response;
using Core.Utility;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthIdentity.BLL.Helper
{
    public static class TokenHelper
    {
        public static string GenerateJwtToken(GetUserDetailsResp user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("abcd");
            var userHash = Encrypt.HybridEncript(StringConst.EncyptSemectrycKey_1, StringConst.EncyptSemectrycKey_2, JsonConvert.SerializeObject(user));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("userhash", userHash) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static GetUserDetailsResp GetUnHashUser(string userHash)
        {
            var key1 = StringConst.EncyptSemectrycKey_1;
            var key2 = StringConst.EncyptSemectrycKey_2;
            //var claimName = context.User.FindFirst(ClaimTypes.Name).Value;
            var userData = JsonConvert.DeserializeObject<GetUserDetailsResp>(Encrypt.HybridDecript(key1, key2, userHash));
            return userData;
        }
    }
}
