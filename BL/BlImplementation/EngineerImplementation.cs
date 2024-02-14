using System.Text.RegularExpressions;

namespace BlImplementation;
using BO;
internal class EngineerImplementation : BlApi.IEngineer
{
	private DalApi.IDal _dal = DalApi.Factory.Get;


	// TODO: because we chack the input here, we should remove it from DAL

	void checkEngineer(BO.Engineer engineer)
	{
		// check validate of engineer:
		if (engineer.Id < 100000000 || engineer.Id > 999999999) throw new BO.BlInvalidParameterException("Id is not valid 9 digit positive number");
		if (string.IsNullOrEmpty(engineer.Name)) throw new BO.BlInvalidParameterException("Name cannot be empty");
		if (engineer.Cost <= 0) throw new BO.BlInvalidParameterException("Our engineers are not slaves");
		if (string.IsNullOrEmpty(engineer.Email)) throw new BO.BlInvalidParameterException("Email cannot be empty");
		// check if the email is valid using Regex:
		if (!new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$").IsMatch(engineer.Email))
			throw new BO.BlInvalidParameterException("invalid email address");
		/*
		// the cool way to check if the email is valid :)
		try
		{
			new System.Net.Mail.MailAddress(engineer.Email);
		}
		catch (FormatException)
		{
			throw new BO.BlInvalidParameterException("invalid email address");
		}
		*/

		// check if task is valid
		if (engineer.Task is not null)
		{
			DO.Task? task = _dal.Task.Read(engineer.Task.Id);
			if (task is null)
				throw new BO.BlDoesNotExistException($"Task with id {engineer.Task.Id} doesn't exist");
		}
	}

	/// <summary> Update task of engineer (old task and new task) </summary>
	/// <param name="engineer"> The engineer to update the task for </param>
	void updateTask(BO.Engineer engineer)
	{
		// unassign the old task
		BO.Task oldTask = BlApi.Factory.Get().Task.Read(engineer.Id)!;
		oldTask.Engineer = null;
		_dal.Task.Update(oldTask.ToDOTask());

		// assign the new task
		if (engineer.Task is not null)
		{
			BO.Task newTask = BlApi.Factory.Get().Task.Read(engineer.Task.Id)!;
			newTask.Engineer = new EngineerInTask() { Id = engineer.Id, Name = engineer.Name };
			_dal.Task.Update(newTask.ToDOTask());
		}
	}

	/// <inheritdoc/>
	public int Create(BO.Engineer engineer)
	{
		checkEngineer(engineer);
		try
		{
			updateTask(engineer);
			return _dal.Engineer.Create(engineer.ToDOEngineer());
			// TODO: if exception thrown, we need to rollback the changes
		}
		catch (DO.DalAlreadyExistsException ex)
		{
			throw new BO.BlAlreadyExistsException(ex.Message);
		}
	}

	/// <inheritdoc/>
	public void Delete(int id)
	{
		try
		{
			// we can delete the engineer
			_dal.Engineer.Delete(id);
		}
		catch (DO.DalDoesNotExistException ex)
		{
			throw new BO.BlDoesNotExistException(ex.Message);
		}
	}

	/// <inheritdoc/>
	public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null)
	{
		IEnumerable<DO.Engineer?> res;

		if (filter is null)
			res = _dal.Engineer.ReadAll();
		else
			res = _dal.Engineer.ReadAll(e => filter(e.ToBOEngineer(_dal)));

		return res.Where(e => e is not null).Select(e => e!.ToBOEngineer(_dal));
	}

	/// <inheritdoc/>
	public BO.Engineer Read(int id)
	{
		DO.Engineer res = _dal.Engineer.Read(id) ?? throw new BO.BlDoesNotExistException($"Engineer with id {id} doesn't exist");
		return res.ToBOEngineer(_dal);
	}

	/// <inheritdoc/>
	public void Update(BO.Engineer engineer)
	{
		checkEngineer(engineer);
		DO.Engineer check = _dal.Engineer.Read(engineer.Id) ?? throw new BO.BlDoesNotExistException($"Engineer with id {engineer.Id} doesn't exist");
		if (check.Level > (DO.EngineerExperience)engineer.Level)
			throw new BO.BlInvalidParameterException("Engineer's level cannot be decreased");

		try
		{
			updateTask(engineer);

			_dal.Engineer.Update(engineer.ToDOEngineer());

			// TODO: if exception thrown, we need to rollback the changes
		}
		catch (DO.DalDoesNotExistException ex)
		{
			throw new BO.BlDoesNotExistException(ex.Message);
		}
	}
}
