namespace DO;

public record Engineer
(
    int Id,
    string Email,
    double Cost,
    string Name,
    EngineerExperience Level
)
{
    public Engineer(int id = 0) : this(id, "", 0, "", EngineerExperience.Beginner) { }
}

