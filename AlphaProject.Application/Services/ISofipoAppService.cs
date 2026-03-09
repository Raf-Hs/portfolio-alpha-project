using AlphaProject.Application.DTOs;

namespace AlphaProject.Application.Services;

public interface ISofipoAppService
{
    Task<IEnumerable<SofipoItemDto>> GetAvailableSofiposAsync();
    Task<SimulationResponseDto> CalculateProjectionAsync(SimulationRequestDto request);
}
