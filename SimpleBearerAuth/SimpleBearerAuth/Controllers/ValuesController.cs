using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace SimpleBearerAuth.Controllers
{
  [ApiController]
  [Authorize]
  public class ValuesController : ControllerBase
  {
    [HttpGet]
    [Route("/values")]
    public ActionResult<IEnumerable<string>> GetList()
    {
      string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

      string userName = User.Identity.Name;

      return new List<string>
      {
        "Sample value 1",
        "Sample value 2",
        userId,
        userName,
      };
    }
  }
}
