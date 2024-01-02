namespace DO;

public record Task
(
 int Id,
 string? Alias,
 string? Description,
 DateTime CreatedAtDate,
 TimeSpan RequiredEffortTime,
  bool IsMilestone,
  EngineerExperience complexity,
  DateTime StartDate,
  DateTime SchedualdDate,
  DateTime DeadLineDate,
  DateTime CompleteDate,
  string Deliverables,
  string Remarks,
  int EngineerId
    )
{

}
