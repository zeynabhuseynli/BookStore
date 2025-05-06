using BookStore.Application.DTOs.UserDtos;
using BookStore.Application.Interfaces.IManagers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.AdminPanel.Controllers;
[Route("api/admin/[controller]")]
[ApiController]
[Authorize(Roles = "Admin, SuperAdmin")]
public class UsersController : Controller
{
    private readonly IUserManager _userManager;

    public UsersController(IUserManager userManager)
    {
        _userManager = userManager;
    }
    [HttpGet("get_user")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        var result= await _userManager.GetByEmailAsync(email);
        return (result!=null) ? Ok(result):Ok(new());
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var results = await _userManager.GetAllUsersAsync();
        return (results != null) ? Ok(results) : Ok(new());
    }

    [HttpGet("deactive_users")]
    public async Task<IActionResult> GetDeactiveUsers()
    {
        var results = await _userManager.GetDeactivatedUsersAsync();
        return (results != null) ? Ok(results) : Ok(new());
    }

    [HttpGet("deleted_users")]
    public async Task<IActionResult> GetDeletedUsers()
    {
        var results = await _userManager.GetSoftDeletedUsersAsync();
        return (results != null) ? Ok(results) : Ok(new());
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> SetUserActivationStatus(int userId,bool isActivated)
    {
        var results = await _userManager.SetUserActivationStatusAsync(userId, isActivated);
        return results ? Ok() : BadRequest();
    }

    [HttpPut("update_role")]
    public async Task<IActionResult> ChangeUserRole(UpdateRoleDto dto)
    {
        var results = await _userManager.UpdateRoleAsync(dto);
        return results ? Ok() : BadRequest();
    }
}

