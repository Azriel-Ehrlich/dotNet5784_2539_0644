using System.Text.RegularExpressions;

namespace BlImplementation;

internal class EngineerImplementation : BlApi.IEngineer
{
	private DalApi.IDal _dal = DalApi.Factory.Get;


	// TODO: because we chack the input here, we should remove it from DAL

	void checkEngineer(BO.Engineer engineer)
	{
		// check validate of engineer:
		if (engineer.Id < 0) throw new BO.BlInvalidParameterException("Id must be positive");
		if (string.IsNullOrEmpty(engineer.Name)) throw new BO.BlInvalidParameterException("Name cannot be empty");
		if (engineer.Cost < 0) throw new BO.BlInvalidParameterException("Our engineers are not slaves");
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
	}


	/// <inheritdoc/>
	public int Create(BO.Engineer engineer)
	{
		checkEngineer(engineer);
		try
		{
			return _dal.Engineer.Create(engineer.ToDOEngineer());
		}
		catch (DO.DalAlreadyExistsException ex)
		{
			throw new BO.BlAlreadyExistsException(ex.Message);
		}
	}

	/// <inheritdoc/>
	public void Delete(int id)
	{
		// check if the engineer is assigned to any tasks
		if (_dal.Task.ReadAll(t => t.EngineerId == id).Any(t => t is not null && t!.Active))
			throw new BO.BlCannotDeleteException($"Engineer with id {id} is still assigned to a task");

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
	public BO.Engineer ReadEngineer(int id)
	{
		DO.Engineer res = _dal.Engineer.Read(id) ?? throw new BO.BlDoesNotExistException($"Engineer with id {id} doesn't exist");
		return res.ToBOEngineer(_dal);
	}

	/// <inheritdoc/>
	public void Update(BO.Engineer engineer)
	{
		checkEngineer(engineer);

		try
		{
			_dal.Engineer.Update(engineer.ToDOEngineer());
		}
		catch (DO.DalDoesNotExistException ex)
		{
			throw new BO.BlDoesNotExistException(ex.Message);
		}
	}
}
