using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MoviesApiProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result=await _authService.RegisterAsync(model);
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);
            return Ok(new {token=result.Token,expiresOn=result.ExpiresOn});
        }
        [HttpPost("login")]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequistModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _authService.GetTokenAsync(model);
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);
            return Ok(new { token = result.Token, expiresOn = result.ExpiresOn });
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("addToRole")]
        public async Task<IActionResult> AddToRoleAsync([FromBody] AddToRoleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result=await _authService.AddToRoleAsync(model);
            if(!string.IsNullOrEmpty(result))
                return BadRequest(result);
            return Ok(model);
        }
    }
}