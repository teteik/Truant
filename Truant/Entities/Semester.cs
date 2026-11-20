namespace Truant.Entities;

public class Semester
{
    public long Id { get; set; }
    public int RandomSeed { get; set; }
    public int Status { get; set; }
    
    public List<ProfessorConfig> ProfessorConfigs { get; set; } = new();

}