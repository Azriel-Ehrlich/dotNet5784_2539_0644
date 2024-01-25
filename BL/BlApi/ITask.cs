namespace BlApi;

internal interface ITask
{
    public int Create(BO.Task task);
    public BO.Task? Read(int id);
    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null);
    public void Update(BO.Task task);
    public void UpdateDate(int id, DateTime date);
}
