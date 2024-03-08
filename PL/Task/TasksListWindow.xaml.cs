using PL.Engineer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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


		/// <summary> for the list of tasks in the window </summary>
		public IEnumerable<BO.TaskInList> TasksList
		{
			get { return (IEnumerable<BO.TaskInList>)GetValue(TasksListProperty); }// return the value of the property
			set { SetValue(TasksListProperty, value); }// set the value of the property
		}

		public static readonly DependencyProperty TasksListProperty = DependencyProperty.Register(
			"TasksList", typeof(IEnumerable<BO.TaskInList>), typeof(TasksListWindow), new PropertyMetadata(null));

		public bool CanAddTasks
		{
			get { return (bool)GetValue(CanAddTasksProperty); }// return the value of the property
			set { SetValue(CanAddTasksProperty, value); }// set the value of the property
		}

		public static readonly DependencyProperty CanAddTasksProperty = DependencyProperty.Register(
			"CanAddTasks", typeof(bool), typeof(TasksListWindow), new PropertyMetadata(null));

		public EngineerExperienceWithAllAndDeleted LevelCategory { get; set; } = EngineerExperienceWithAllAndDeleted.All;


		public TasksListWindow()
		{
			InitializeComponent();
			UpdateTasksList();
			CanAddTasks = !s_bl.IsProjectScheduled();
		}

		/// <summary> update the list of tasks in the window according to the selected level </summary>
		void UpdateTasksList()
		{
			TasksList = (
				(LevelCategory == EngineerExperienceWithAllAndDeleted.All) ? s_bl?.Task.ReadAll(t => t.IsActive)!
				: (LevelCategory == EngineerExperienceWithAllAndDeleted.Deleted) ? s_bl?.Task.ReadAll(t => !t.IsActive)!
			   : s_bl?.Task.ReadAll(t => t.IsActive && t.Complexity is not null && t.Complexity == (BO.EngineerExperience?)LevelCategory)!
			   ).OrderBy(e => e.Id); // sort by ID so it will be easier to find the task in the list
		}
		/// <summary>
		/// change the level of the task
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ChangeToSelectedLevel(object sender, SelectionChangedEventArgs e)
		{
			UpdateTasksList();
		}
		/// <summary>
		/// adding a task
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AddTask(object sender, RoutedEventArgs e)
		{
			if (TasksList.Any(t => t.Status != BO.Status.Unscheduled))
			{
				MessageBox.Show("You can't add a new task after the schdule date");
				return;
			}
			new TaskDataInputWindow().ShowDialog();
			UpdateTasksList();
		}
		/// <summary>
		/// updating a task
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void UpdateTask(object sender, MouseButtonEventArgs e)
		{
			if ((sender as ListView)?.SelectedItem is BO.TaskInList task)
			{
				if (task is not null)
				{
					try
					{
						new TaskDataInputWindow(task.Id).ShowDialog();
						UpdateTasksList();
					}
					catch (Exception) { }
				}
			}
		}
	}
}
