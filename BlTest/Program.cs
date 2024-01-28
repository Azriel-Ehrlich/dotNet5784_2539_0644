using BlApi;

namespace BlTest;

internal class Program
{
	static IBl bl = new BlImplementation.Bl();

	class A
	{
		public int i { get; set; }
		public int j { get; set; }

		public List<int> ints { get; set; }

		public string str { get; set; }

	//	public override string ToString() => this.ToStringProperty();

	}

	static void Main(string[] args)
	{
		//Console.WriteLine(a);

		// simple test for reflection to string
		BO.Engineer e = new()
		{
			Id = 347655849,
			Name = "Moshe",
			Email = "Moshe@gmail.com",
			Cost = 20.44,
			Level = BO.EngineerExperience.Beginner,
			Active = true,
			Task = new List<BO.TaskInEngineer>()
			{
				new BO.TaskInEngineer()
				{
					Id = 1,
					Alias = "Task1"
				},
				new BO.TaskInEngineer()
				{
					Id = 2,
					Alias = "Task2"
				},
				new BO.TaskInEngineer()
				{
					Id = 2,
					Alias = "Task2"
				},
				new BO.TaskInEngineer()
				{
					Id = 2,
					Alias = "Task2"
				},
				new BO.TaskInEngineer()
				{
					Id = 2,
					Alias = "Task2"
				},
				new BO.TaskInEngineer()
				{
					Id = 2,
					Alias = "Task2"
				}
			}
		};
		Console.WriteLine(e);
		//Console.WriteLine(BlImplementation.Tools.ToStringProperty("bfdhd"));
	}
}
