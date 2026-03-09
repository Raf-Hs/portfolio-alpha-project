using AlphaProject.Application.DTOs;
using AlphaProject.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlphaProject.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SofiposController(ISofipoAppService appService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetSofipos()
    {
        var sofipos = await appService.GetAvailableSofiposAsync();
        return Ok(sofipos);
    }

    [HttpPost("simulate")]
    public async Task<IActionResult> Simulate([FromBody] SimulationRequestDto request)
    {
        try
        {
            var result = await appService.CalculateProjectionAsync(request);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}
