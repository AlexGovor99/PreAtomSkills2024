using Microsoft.AspNetCore.Mvc;
using OrderStore.Contracts;

namespace OrderStore.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    public AuthController()
    {
    }

    [HttpPost("login")]
    public Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var user = Users.Users.SystemUsers.FirstOrDefault(
            x => x.Login.Equals(request.username, StringComparison.InvariantCultureIgnoreCase)
              && x.Password.Equals(request.password, StringComparison.InvariantCulture));

        if (user == null)
        {
            return Task.FromResult<ActionResult<LoginResponse>>(Unauthorized());
        }

        return Task.FromResult<ActionResult<LoginResponse>>(Ok(new LoginResponse(user.Id, user.Role)));
    }

    [HttpPost("logout")]
    public Task<ActionResult<string>> Logout()
    {
        return Task.FromResult<ActionResult<string>>(Ok("OK"));
    }
}