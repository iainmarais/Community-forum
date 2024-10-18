/*
This helper class is used to validate bearer tokens.
It is a workaround for the bug introduced in .net 8 with validation.

Change the namespace to that of your project -> security or whatever subdirectory you have your security modules in.
namespace YourProject.Security //block scope namespace
{
    //Code here.
}
//--or--
namespace YourProject.Security; //file scope namespace
*/
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace RestApiServer.Security
{
    public class BearerTokenHandler: TokenHandler
    {   
        private readonly JwtSecurityTokenHandler _tokenHandler = new();

        public override Task<TokenValidationResult> ValidateTokenAsync(string token, TokenValidationParameters validationParameters)
        {
            try
            {
                _tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                if(validatedToken is not JwtSecurityToken jwtSecurityToken)
                {
                    return Task.FromResult(new TokenValidationResult(){ IsValid = false });
                }
                return Task.FromResult(new TokenValidationResult
                { 
                    IsValid = true,
                    ClaimsIdentity = new ClaimsIdentity(jwtSecurityToken.Claims, JwtBearerDefaults.AuthenticationScheme),

                    SecurityToken = jwtSecurityToken
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new TokenValidationResult
                {
                    IsValid = false,
                    Exception = ex
                });
            }
        }
    }
}