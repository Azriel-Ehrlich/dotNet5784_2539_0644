namespace BlApi;

public  interface IEngineer
{
    public int Create(BO.Engineer engineer);
    public BO.Engineer? Engineer(int id);
    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer,bool>?filter=null);
    public void Update(BO.Engineer engineer);
    public void Delete(int id);
   // public IEnumerable<BO.EngineerInTask> ReadAllEngineersInTask(int taskId);

}
