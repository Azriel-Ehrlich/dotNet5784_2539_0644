namespace BO;
/// <summary>MileStone </summary>
///< param name="Id">MileStone's unique id</param>
/// <param name="Description">MileStone's description</param>
/// <param name="Alias">MileStone's alias</param>
///  <param name="CreatedAtDate">MileStone's creation date</param>
///  <param name="Status">MileStone's status</param>
///<param name="ForecastedDate">MileStone's forecasted end date</param>
///<param name="DeadLine">MileStone's deadline</param>
///<param name="CompleteDate">MileStone's actual completion date</param>
///<param name="CompletionPercentage">MileStone's completion percentage</param>
///<param name="Remarks">free remarks from project meetings</param>
///<param name="Dependencies">MileStone's dependencies</param>
public class MileStone
{

    public int Id { init; get; }
    public required string Description { set; get; }
    public required string  Alias { set; get; }
    public DateTime CreatedAtDate { init; get; }
    public Status Status { set; get; }
    public DateTime ForecastedDate { set; get; }
    public DateTime DeadLine { set; get; }
    public DateTime CompleteDate { set; get; }
    public double CompletionPercentage { set; get; }
    public string? Remarks { set; get; }
    public required List<BO.TaskInList>Dependencies { set; get; }
}
