namespace Truant.Shared.Dto;

public record RecordQuestionRequest
{
    public int Day { get; init; }
    public string Subject { get; init; } = string.Empty;
    public bool WasAsked { get; init; }
}
