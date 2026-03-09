// AlphaProject.Application/DTOs/ProjectionRowDto.cs
namespace AlphaProject.Application.DTOs
{
    public record ProjectionRowDto(
        int Period, 
        decimal StartingBalance, 
        decimal InterestEarned, 
        decimal EndingBalance
    );
}