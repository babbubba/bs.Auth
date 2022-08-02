using bs.Auth.Api.AuthMock;
using bs.Auth.Interfaces.Services;
using bs.Auth.Interfaces.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bs.Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Auth : ControllerBase
    {
        private readonly IAuthService authService;

        public Auth(IAuthService authService)
        {
            this.authService = authService;
        }
        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate(UserAuthDto userAuth)
        {
            var result = await authService.AuthenticateAsync<int>(userAuth);

            return Ok(new Response<IUserViewModel<int>>(result));

        }
    }
}
