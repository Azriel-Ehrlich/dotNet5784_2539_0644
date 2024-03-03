namespace BO;

// NOTE: The next exceptions same to DO, but they are in BO because they are used in BlApi.


/// <summary> exception for when a BL doesn't exist for read, update or delete </summary>
[Serializable]
public class BlDoesNotExistException : Exception
{
	public BlDoesNotExistException(string? message) : base(message) { }
}

/// <summary> exception for when a BL already exists for create </summary>
[Serializable]
public class BlAlreadyExistsException : Exception
{
	public BlAlreadyExistsException(string? message) : base(message) { }
}

/// <summary> exception for when a BL object cannot be deleted </summary>
[Serializable]
public class BlCannotDeleteException : Exception
{
	public BlCannotDeleteException(string? message) : base(message) { }
}

/// <summary> exception for when a parameter to BL method is invalid </summary>
[Serializable]
public class BlInvalidParameterException : Exception
{
	public BlInvalidParameterException(string? message) : base(message) { }
}

/// <summary> exception for when a property of a BL object is null
[Serializable]
public class BlNullPropertyException : Exception
{
	public BlNullPropertyException(string? message) : base(message) { }
}

/// <summary> exception for when the BL object cannot be updated </summary>
[Serializable]
public class BlCannotUpdateException : Exception
{
	public BlCannotUpdateException(string? message) : base(message) { }
}

/// <summary> exception for when a dal has invalid input for test program </summary>
[Serializable]
public class BlInvalidInputException : Exception
{
	public BlInvalidInputException(string? message = "invalid input") : base(message) { }
}

/// <summary> exception ehen cannot create a new object </summary>
[Serializable]
public class BlCannotCreateException : Exception
{
	public BlCannotCreateException(string? message) : base(message) { }
}
