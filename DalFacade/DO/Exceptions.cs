namespace DO;

/// <summary> exception for when a dal doesn't exist for read, update or delete </summary>
[Serializable]
public class DalDoesNotExistException : Exception
{
	public DalDoesNotExistException(string? message) : base(message) { }
}

/// <summary> exception for when a dal already exists for create </summary>
[Serializable]
public class DalAlreadyExistsException : Exception
{
	public DalAlreadyExistsException(string? message) : base(message) { }
}

/// <summary> exception for when a dal has invalid input for test program </summary>
[Serializable]
public class DalInvalidInputException : Exception
{
	public DalInvalidInputException(string? message = "invalid input") : base(message) { }
}

/// <summary> exception for when a dal can't load or create a XML file </summary>
[Serializable]
public class DalXMLFileLoadCreateException : Exception
{
	public DalXMLFileLoadCreateException(string? message = "invalid input") : base(message) { }
}
