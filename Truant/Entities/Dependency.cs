namespace Truant.Entities;

public class Dependency
{
    public long Id { get; set; }
    public long ProfessorId { get; set; }
    public long DependentProfessorId { get; set; }
}