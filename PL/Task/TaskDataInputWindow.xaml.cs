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
	/// Interaction logic for TaskDataInputWindow.xaml
	/// This window allow us get task's data from the user with awesome GUI
	/// </summary>
	public partial class TaskDataInputWindow : Window
	{
		static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

		public class CurrentTaskType // for binding
		{
			public BO.Task Task { get; set; }
			public bool isNewTask { set; get; } // save the state: create new or update engineer

			public CurrentTaskType(int id)
			{
				isNewTask = (id == -1);
				Task = isNewTask ? new BO.Task() { Alias = "", Description = "" } : s_bl.Task.Read(id);
				if (Task.Engineer is null)
				{
					Task.Engineer = new BO.EngineerInTask() { Name = "" };
				}
			}
		}

		public CurrentTaskType CurrentTask
		{
			get { return (CurrentTaskType)GetValue(CurrentTaskProperty); }
			set { SetValue(CurrentTaskProperty, value); }
		}

		// Using a DependencyProperty as the backing store for CurrentTask. This enables animation, styling, binding, etc...
		public static readonly DependencyProperty CurrentTaskProperty = DependencyProperty.Register("CurrentTask", typeof(CurrentTaskType), typeof(TaskDataInputWindow), new PropertyMetadata(null));


		public TaskDataInputWindow(int Id = -1)
		{
			InitializeComponent();
			CurrentTask = new(Id);
		}

		//  TODO: read dates

		private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (CurrentTask.isNewTask)
				{
					s_bl.Task.Create(CurrentTask.Task);
					MessageBox.Show("Task added successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
				}
				else
				{
					s_bl.Task.Update(CurrentTask.Task);
					// TODO: use this "s_bl.SuggestedDate"
					MessageBox.Show("Task updated successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
				}

				this.Close(); // close window after adding or updating the Task
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

        private void AddDependencies(object sender, RoutedEventArgs e)
        {
			new DependenciesWindow(CurrentTask.Task.Id).ShowDialog();
			CurrentTask.Task= s_bl.Task.Read(CurrentTask.Task.Id);

        }
    }
}
