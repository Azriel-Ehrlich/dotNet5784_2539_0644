using PL.Engineer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Task
{
	/// <summary>
	/// Interaction logic for TasksListWindow.xaml
	/// </summary>
	public partial class TasksListWindow : Window
	{
		static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

		public TasksListWindow()
		{
			InitializeComponent();
			UpdateTasksList();
		}

		/// <summary> for the list of engineers in the window </summary>
		public IEnumerable<BO.TaskInList> TasksList
		{
			get { return (IEnumerable<BO.TaskInList>)GetValue(TasksListProperty); }// return the value of the property
			set { SetValue(TasksListProperty, value); }// set the value of the property
		}

		public static readonly DependencyProperty TasksListProperty = DependencyProperty.Register(
			"TasksList", typeof(IEnumerable<BO.TaskInList>), typeof(TasksListWindow), new PropertyMetadata(null));

		public BO.EngineerExperienceWithAll LevelCategory { get; set; } = BO.EngineerExperienceWithAll.All;

		/// <summary> update the list of engineers in the window according to the selected level </summary>
		void UpdateTasksList()
		{
			TasksList = (
				(LevelCategory == BO.EngineerExperienceWithAll.All) ? s_bl?.Task.ReadAll()!
			   : s_bl?.Task.ReadAll(item => item.Complexity is not null && item.Complexity == (BO.EngineerExperience?)LevelCategory)!
			   ).OrderBy(e => e.Id); // sort by ID so it will be easier to find the engineer in the list as a human
		}

		private void ChangeToSelectedLevel(object sender, SelectionChangedEventArgs e)
		{
			UpdateTasksList();
		}

		private void AddTask(object sender, RoutedEventArgs e)
		{
			new TaskDataInputWindow().ShowDialog();
			UpdateTasksList();
		}

		private void UpdateTask(object sender, MouseButtonEventArgs e)
		{
			if ((sender as ListView)?.SelectedItem is BO.TaskInList task)
			{
				new TaskDataInputWindow(task.Id).ShowDialog();
				UpdateTasksList();
			}
		}
	}
}
