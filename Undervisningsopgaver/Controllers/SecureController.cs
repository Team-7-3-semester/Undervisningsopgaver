using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Undervisningsopgaver.DTOs;

namespace Undervisningsopgaver.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SecureController : ControllerBase
{
    // Offentligt endpoint - ingen autentificering nødvendig
    [HttpGet("public")]
    [AllowAnonymous]
    public IActionResult PublicEndpoint()
        => Ok(new { Message = "Dette er offentligt" });

    // Kræver blot at man er logget ind
    [Authorize]
    [HttpGet("private")]
    public IActionResult PrivateEndpoint()
    {
        // User er automatisk tilgængelig når [Authorize] er opfyldt
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        return Ok(new { Message = "Dette er privat", UserId = userId, Email = email });
    }

    // Kun Admin-rolle
    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public IActionResult AdminEndpoint()
        => Ok(new { Message = "Velkommen, Admin!" });

    // Enten Admin eller Manager
    [Authorize(Roles = "Admin,Manager")]
    [HttpGet("management")]
    public IActionResult ManagementEndpoint()
        => Ok(new { Message = "Adgang til ledelse" });
}