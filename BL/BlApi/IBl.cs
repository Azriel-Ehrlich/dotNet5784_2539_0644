using BO;

namespace BlApi;

public interface IBl
{
    public IEngineer Engineer { get; }
    public ITask Task { get; }
    public DateTime? SuggestedDate(BO.Task? task, DateTime startProj)
    {
        DateTime? date = startProj;
        if (task!.Dependencies is null) return date;
        foreach (var dep in task!.Dependencies!)
        {
            BO.Task? depTask = Task.Read(dep.Id);
            if (depTask!.ScheduledDate is null) throw new BlCannotUpdateException("The task cann't be updated");
            DateTime? depSDate = depTask.ScheduledDate + depTask.RequiredEffortTime;
            DateTime? depTDate = depTask.StartDate + depTask.RequiredEffortTime;
            if (depSDate > date)
                date = depSDate;
            if (depTDate > date && depTDate is not null)
                date = depTDate;
        }
        return date;
    }
}
