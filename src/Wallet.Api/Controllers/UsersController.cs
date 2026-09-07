using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Users.Registration;

namespace Wallet.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly RegisterUserHandler _userHandler;

        public UsersController(RegisterUserHandler userHandler)
        {
            _userHandler = userHandler;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterUserCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _userHandler.Handle(
                command, 
                cancellationToken);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessages);

            return StatusCode(
                StatusCodes.Status201Created, 
                result.Value);
        }
    }
}
