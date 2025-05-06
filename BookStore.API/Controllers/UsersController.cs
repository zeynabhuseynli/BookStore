using BookStore.Application.DTOs.UserDtos;
using BookStore.Application.Interfaces.IManagers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserManager _userManager;

    public UsersController(IUserManager userManager)
    {
        _userManager = userManager;
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var result = await _userManager.GetCurrentUserAsync();
        return (result != null) ? Ok(result) : Ok(new());
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var result = await _userManager.RegisterAsync(dto);
        if (result) return Ok();
        return BadRequest();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var tokens = await _userManager.LoginAsync(dto);
        if (tokens == null) return Unauthorized("Email or password is not true!");
        return Ok(tokens);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> SendForgotPasswordOtp([FromBody] string email)
    {
        await _userManager.SendForgotPasswordOtpAsync(email);
        return Ok();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        var result = await _userManager.ResetPasswordWithOtpAsync(dto);
        if (result) return Ok();
        return BadRequest();
    }

    [Authorize]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateUserDto dto)
    {
        var result = await _userManager.UpdateUserInfoAsync(dto);
        if (result) return Ok();
        return NotFound();
    }

    [Authorize]
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var result = await _userManager.ChangePasswordAsync(dto);
        if (result) return Ok();
        return NotFound();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> SoftDelete(int id)
    {
        var result = await _userManager.SoftDeleteAsync(id);
        if (result) return Ok();
        return NotFound();
    }
}

