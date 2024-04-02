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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL.Task
{
	/// <summary>
	/// Interaction logic for TaskDataInputWindow.xaml
	/// This window allow us get task's data from the user with awesome GUI
	/// </summary>
	public partial class TaskDataInputWindow : Window
	{
		static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

		public class TaskInDepList : BO.TaskInList
		{
			public bool IsChecked { get; set; }
		}

		public IEnumerable<BO.TaskInList> AllTasks
		{
			get { return (IEnumerable<BO.TaskInList>)GetValue(AllTasksProperty); }
			set { SetValue(AllTasksProperty, value); }
		}

		public BO.Task CurrentTask
		{
			get { return (BO.Task)GetValue(CurrentTaskProperty); }
			set { SetValue(CurrentTaskProperty, value); }
		}

		// Using a DependencyProperty as the backing store for CurrentTask. This enables animation, styling, binding, etc...
		public static readonly DependencyProperty CurrentTaskProperty = DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(TaskDataInputWindow), new PropertyMetadata(null));
		public static readonly DependencyProperty AllTasksProperty = DependencyProperty.Register("AllTasks", typeof(IEnumerable<BO.TaskInList>), typeof(TaskDataInputWindow), new PropertyMetadata(null));


		public TaskDataInputWindow(int Id = -1)
		{
			InitializeComponent();

			CurrentTask = (Id != ConstantValues.NO_ID) ? s_bl.Task.Read(Id)
					: new BO.Task() { Id = Id, Alias = "", Description = "", IsActive = true };
			if (CurrentTask.Engineer is null)
			{
				CurrentTask.Engineer = new BO.EngineerInTask() { Name = "" };
			}

			AllTasks = s_bl.Task.ReadAll().OrderBy(t => t.Id).Select(t =>
				new TaskInDepList()
				{
					Id = t.Id,
					Description = t.Description,
					Alias = t.Alias,
					Status = t.Status,
					IsChecked = CurrentTask.Dependencies?.Any(d => d.Id == t.Id) ?? false
				});

		}

		private void AddOrUpdateTask(object sender, RoutedEventArgs e)
		{
			try
			{
				if (CurrentTask.Id == ConstantValues.NO_ID)
				{
					s_bl.Task.Create(CurrentTask);
					MessageBox.Show("Task added successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
				}
				else
				{
					s_bl.Task.Update(CurrentTask);
					MessageBox.Show("Task updated successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
				}

				this.Close(); // close window after adding or updating the Task
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void AddDependency(object sender, RoutedEventArgs e)
		{
			if (CurrentTask.IsActive)
			{
				BO.TaskInList task = ((sender as CheckBox)!.DataContext as BO.TaskInList)!;
				CurrentTask.Dependencies ??= new();

				// check if task already exists in the dependencies list
				if (CurrentTask.Dependencies.Any(t => t.Id == task.Id))
					return;

				CurrentTask.Dependencies.Add(task);
			}
		}
		private void RemoveDependency(object sender, RoutedEventArgs e)
		{
			if (CurrentTask.IsActive)
			{
				BO.TaskInList task = ((sender as CheckBox)!.DataContext as BO.TaskInList)!;
				CurrentTask.Dependencies ??= new();
				CurrentTask.Dependencies = CurrentTask.Dependencies.Where(t => t.Id != task.Id).ToList();
			}
		}
		/// <summary>
		/// deleting or restoring a deleted task
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ChangeTaskState(object sender, RoutedEventArgs e)
		{
			try
			{
				string text = CurrentTask.IsActive ? "delete" : "restore";

				var result = MessageBox.Show($"Are you sure you want to {text} this task?", $"{text} task", MessageBoxButton.YesNo, MessageBoxImage.Question);
				if (result == MessageBoxResult.No)
					return;

				if (CurrentTask.IsActive)
					s_bl.Task.Delete(CurrentTask.Id);
				else
					s_bl.Task.Restore(CurrentTask.Id);
			
				MessageBox.Show($"Task {text}d successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
				this.Close(); // close window after deleting/restoring the Task
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}
