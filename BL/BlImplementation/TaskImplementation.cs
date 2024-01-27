using BlApi;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace BlImplementation;

// TODO: Implement this class

internal class TaskImplementation : BlApi.ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    void checkTask(BO.Task task)
    {
        if (string.IsNullOrEmpty(task.Alias)) throw new BO.BlInvalidParameterException("Alias cannot be empty");
        if (string.IsNullOrEmpty(task.Description)) throw new BO.BlInvalidParameterException("Description cannot be empty");
        if (TimeSpan.Compare(TimeSpan.Zero, task.RequiredEffortTime) != -1) throw new BO.BlInvalidParameterException("RequiredEffortTime cannot be negative or zero");
        if (task.StartDate > task.DeadlineDate) throw new BO.BlInvalidParameterException("StartDate cannot be after DeadlineDate");
        if (task.StartDate > task.ForecastDate) throw new BO.BlInvalidParameterException("StartDate cannot be after ForecastDate");
        if (task.StartDate > task.CompleteDate) throw new BO.BlInvalidParameterException("StartDate cannot be after CompleteDate");
        if (task.Engineer is not null)
        {
            DO.Engineer? eng = _dal.Engineer.Read(task.Engineer.Id);
            if (eng is null) throw new BO.BlInvalidParameterException("The Engineer assigned to the task doesn't exist");

        }
    }


    /// <inheritdoc/>
    /// Ariel please check the dependencies
    public int Create(BO.Task task)
    {
        checkTask(task);
        IEnumerable<DO.Task?> test = _dal.Task.ReadAll();
        DO.Task? check = (from t in test
                          where t.Alias == task.Alias
                          select t).FirstOrDefault();
        if (check is not null) throw new BO.BlAlreadyExistsException("The task already exist");
        if (task.Dependencies is not null)
        { // check if the dependencies exist and if not create them
            IEnumerable<int> d = from t in task.Dependencies
                                 select t.Id;
            IEnumerable<DO.Dependency> dep = (from t in d
                                              where _dal.Task.Read(t) is not null
                                              select new DO.Dependency()
                                              { DependentTask = task.Id, DependsOnTask = t });
            IEnumerable<int> stam = from t in dep select _dal.Dependency.Create(t);
        }
            return _dal.Task.Create(task.ToDOTask());
        }
        ///<inheritdoc/>
        public void Delete(int id)
        {
            DO.Task? eng = _dal.Task.Read(id);
            if (eng is null) throw new BO.BlDoesNotExistException("The task doesn't exist");
            IEnumerable<DO.Dependency?> test = _dal.Dependency.ReadAll();
            DO.Dependency? check = (from t in test// check if there is another task teat depends on this task
                                    where t.DependsOnTask == id
                                    select t).FirstOrDefault();
            if (check is not null) throw new BO.BlCannotDeleteException("The task cann't be deleted");
            try
            {
                _dal.Task.Delete(id);
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BlDoesNotExistException(ex.Message);
            }
        }
        /// <inheritdoc/>
        public BO.Task? Read(int id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Update(BO.Task task)
        {
            IEnumerable<DO.Task?> test = _dal.Task.ReadAll();
            DO.Task? check = (from t in test
                              where t.Alias == task.Alias
                              select t).FirstOrDefault();
            if (check is null) throw new BO.BlDoesNotExistException("The task doesn't exist");
            DO.Task eng = task.ToDOTask();
            _dal.Task.Update(eng);

        }

        /// <inheritdoc/>
        public void UpdateDate(int id, DateTime date)
        {
        DO.Task? tas = _dal.Task.Read(id);
        if(tas is null) throw new BO.BlDoesNotExistException("The task doesn't exist");
        IEnumerable<DO.Dependency?> test = _dal.Dependency.ReadAll(d=>d.DependentTask==id);
        if (test is not null)
        {
            IEnumerable<DO.Task?>check= from t in test
                                         where t.DependentTask == id
                                         select _dal.Task.Read((int)t.DependsOnTask!);
            if((from t in check where t.StartDate==null select t).Any()) throw new BO.BlCannotUpdateException("The task cann't be updated");
            if ((from t in check where (t.StartDate+t.RequiredEffortTime) > date select t).Any()) throw new BO.BlCannotUpdateException("The task cann't be updated");
        }
        DO.Task upd=tas with { ScheduledDate = date };
        _dal.Task.Update(upd);
    }




    }
