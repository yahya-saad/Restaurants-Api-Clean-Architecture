using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Infrastructure.Authorization;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestingController : ControllerBase
{
    [HttpGet("policy/hasNationality")]
    [Authorize(Policy = PolicyNames.HasNationality)]
    public IActionResult HasNationalityPolicy(string? Nationality)
    {
        return Ok(Nationality);

    }

    [HttpGet("policy/Atleast18")]
    [Authorize(Policy = PolicyNames.AtLeast18)]
    public IActionResult Atleast18Policy()
    {
        return Ok();

    }

    [HttpGet("policy/combine")]
    [Authorize(Policy = PolicyNames.HasNationality)]
    [Authorize(Policy = PolicyNames.AtLeast18)]
    public IActionResult Combine()
    {
        return Ok();

    }
}
