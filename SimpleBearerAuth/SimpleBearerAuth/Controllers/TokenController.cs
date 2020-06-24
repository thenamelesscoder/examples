using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SimpleBearerAuth.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimpleBearerAuth.Controllers
{
  [ApiController]
  public class TokenController : ControllerBase
  {
    [HttpPost]
    [Route("/token")]
    public ActionResult<string> Create([FromBody] TokenForCreateDto dto)
    {
      // a fake valid user, this should be replace with your database calls for example
      var validUser = new
      {
        Id = "bd25d93b-5366-44b8-8232-a06cf103d488",
        MailAddress = "hello@example.com",
        Password = "super-secure-password",
        Role = "PayingUser",
      };

      if (dto.MailAddress != validUser.MailAddress || dto.Password != validUser.Password)
      {
        // if this isn't a valid token request, reject the request
        return BadRequest();
      }

      // generate the claims array for the token payload
      Claim[] claims = new Claim[]
      {
        new Claim(ClaimTypes.NameIdentifier, validUser.Id), // the user identifier
        new Claim(ClaimTypes.Name, validUser.MailAddress), // the user name
        new Claim(ClaimTypes.Role, validUser.Role), // the user role
        new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()), // token valid since
        new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(28)).ToUnixTimeSeconds().ToString()), // token expires on
      };

      // generate a key for the signature of the token
      SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySymmetricSecurityKey")); // <-- this string should be replaced with a secret

      // create the signing credentials for the token header
      SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      // create a new token with the header and payload we just defined
      JwtSecurityToken token = new JwtSecurityToken(
        new JwtHeader(signingCredentials),
        new JwtPayload(claims)
      );

      // serialize the token into a base64 encoded string as needed
      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}
