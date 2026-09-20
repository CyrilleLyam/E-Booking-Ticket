using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using server.src.Dtos;
using server.src.Services;

namespace server.src.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<BaseResponse<AuthResponseDto>>> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            var result = await _authService.Register(registerDto);
            return Ok(new BaseResponse<AuthResponseDto>(result));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = true, status = StatusCodes.Status400BadRequest, message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<BaseResponse<AuthResponseDto>>> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var result = await _authService.Login(loginDto);
            return Ok(new BaseResponse<AuthResponseDto>(result));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = true, status = StatusCodes.Status401Unauthorized, message = ex.Message });
        }
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<BaseResponse<AuthResponseDto>>> Refresh([FromBody] RefreshTokenDto refreshTokenDto)
    {
        try
        {
            var result = await _authService.RefreshToken(refreshTokenDto);
            return Ok(new BaseResponse<AuthResponseDto>(result));
        }
        catch (SecurityTokenException ex)
        {
            return Unauthorized(new { error = true, status = StatusCodes.Status401Unauthorized, message = ex.Message });
        }
    }

    [HttpPost("revoke")]
    public async Task<IActionResult> Revoke([FromBody] RefreshTokenDto refreshTokenDto)
    {
        await _authService.RevokeToken(refreshTokenDto);
        return NoContent();
    }
}
