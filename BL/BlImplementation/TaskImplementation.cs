namespace BlImplementation;

// TODO: Implement this class

internal class TaskImplementation : BlApi.ITask
{
	/// <inheritdoc/>
	public int Create(BO.Task task)
	{
		throw new NotImplementedException();
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
		throw new NotImplementedException();
	}

	/// <inheritdoc/>
	public void UpdateDate(int id, DateTime date)
	{
		throw new NotImplementedException();
	}
}
