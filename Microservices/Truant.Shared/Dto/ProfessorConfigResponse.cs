namespace Truant.Shared.Dto;

public record ProfessorConfigResponse
{
    public string Subject { get; init; } = string.Empty;
    public int RuleType { get; init; }
    public string? Dependencies { get; init; }
    public int BaseSeed { get; init; }
}