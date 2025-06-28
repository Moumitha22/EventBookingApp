using EventBookingApi.Interfaces;
using EventBookingApi.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authService;
    private readonly IApiResponseMapper _responseMapper;
    private readonly IWebHostEnvironment _env;

    public AuthenticationController(
        IAuthenticationService authService,
        IApiResponseMapper responseMapper,
        IWebHostEnvironment env)
    {
        _authService = authService;
        _responseMapper = responseMapper;
        _env = env;
    }

    private CookieOptions GetRefreshTokenCookieOptions()
    {
        return new CookieOptions
        {
            HttpOnly = true,
            Secure = !_env.IsDevelopment(),
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        };
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequestDto request)
    {
        var result = await _authService.RegisterAsync(request);
        var response = _responseMapper.MapToOkResponse("User registered successfully", result);
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequestDto loginRequest)
    {
        var loginResponse = await _authService.LoginAsync(loginRequest);
        Response.Cookies.Append("refreshToken", loginResponse.RefreshToken, GetRefreshTokenCookieOptions());
        loginResponse.RefreshToken = null;
        var response = _responseMapper.MapToOkResponse("Login successful", loginResponse);
        return Ok(response);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var token = Request.Cookies["refreshToken"];
        if (string.IsNullOrWhiteSpace(token))
            return BadRequest(_responseMapper.MapToErrorResponse<object>(400, "Missing refresh token"));

        var tokens = await _authService.RefreshTokenAsync(token);
        Response.Cookies.Append("refreshToken", tokens.RefreshToken, GetRefreshTokenCookieOptions());
        tokens.RefreshToken = null;
        var response = _responseMapper.MapToOkResponse("Token refreshed successfully", tokens);
        return Ok(response);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (!string.IsNullOrWhiteSpace(refreshToken))
        {
            await _authService.LogoutAsync(refreshToken);
        }

        Response.Cookies.Append("refreshToken", "", new CookieOptions
        {
            HttpOnly = true,
            Secure = !_env.IsDevelopment(),
            SameSite = SameSiteMode.Strict,
            Path = "/",
            Expires = DateTimeOffset.UtcNow.AddDays(-1)
        });

        var response = _responseMapper.MapToOkResponse("Logged out successfully");
        return Ok(response);
    }
}
