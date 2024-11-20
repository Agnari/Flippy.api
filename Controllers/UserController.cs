using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

[Route("api/User")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserController(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    [Authorize]
    [HttpGet("username")]
    public async Task<IActionResult> GetUsername()
    {
        // Get the user ID from the authenticated token
        var userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

        if (userId == null)
        {
            return Unauthorized("User is not authenticated.");
        }

        // Retrieve user information from the database
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        return Ok(new
        {
            Username = user.UserName,
            Email = user.Email
        });
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid request data.");
        }

        // Get the user ID from the authenticated token
        var userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        if (userId == null)
        {
            return Unauthorized("User is not authenticated.");
        }

        // Find the user in the database
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        // Attempt to change the user's password
        var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

        if (result.Succeeded)
        {
            return Ok("Password changed successfully.");
        }

        return BadRequest(new { Message = "Failed to change password.", Errors = result.Errors });
    }

    public class ChangePasswordModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

}
