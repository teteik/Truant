namespace Truant.Shared.Dto;

public record RecordAttendanceRequest
{
    public int Day { get; init; }
    public string Subject { get; init; } = string.Empty;
    public bool Attended { get; init; }
}