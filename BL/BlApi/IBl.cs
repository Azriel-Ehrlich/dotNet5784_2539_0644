using BO;

namespace BlApi;

public interface IBl
{
    public IEngineer Engineer { get; }
    public ITask Task { get; }
    public DateTime? SuggestedDate(BO.TaskInList? task, DateTime startProj)
    {
        BO.Task? tas= Task.Read(task!.Id);
        DateTime? date = startProj;
        if (tas!.Dependencies is null) return date;
        foreach (var dep in tas!.Dependencies!)
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
