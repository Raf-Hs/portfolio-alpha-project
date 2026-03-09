// AlphaProject.Api/Controllers/ProjectionsController.cs
using AlphaProject.Application.DTOs;
using AlphaProject.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlphaProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectionsController(IProjectionAppService appService) : ControllerBase
    {
        [HttpPost("compound-interest")]
        public async Task<IActionResult> CalculateCompoundInterest([FromBody] CalculateProjectionRequest request)
        {
            try
            {
                var result = await appService.GenerateProjectionAsync(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                // Manejo básico de errores de validación del dominio
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}