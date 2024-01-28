using BlImplementation;

namespace BO;

public class MilestoneInList
{
    public int Id { init; get; }
    public required string Description { set; get; }
    public required string Alias { set; get; }
    public Status Status { set; get; }
    public double CompletionPrecentage { set; get; }


	public override string ToString() => this.ToStringProperty();
}
