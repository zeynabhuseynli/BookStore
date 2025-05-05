using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Application.Interfaces.IManagers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookStore.AdminPanel.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserManager _userManager;

        public UsersController(IUserManager userManager)
        {
            _userManager = userManager;
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
}

