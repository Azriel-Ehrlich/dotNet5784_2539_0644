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

namespace PL.Engineer
{
	/// <summary> Interaction logic for EngineerWindow.xaml </summary>
	public partial class EngineerWindow : Window
	{
		static readonly BlApi.IBl s_bl = BlApi.Factory.Get();


		public BO.Engineer Engineer
		{
			get { return (BO.Engineer)GetValue(EngineerProperty); }
			set { SetValue(EngineerProperty, value); }
		}

		public IEnumerable<BO.TaskInList> TasksList
		{
			get { return (IEnumerable<BO.TaskInList>)GetValue(TasksListProperty); }
			set { SetValue(TasksListProperty, value); }
		}

		// Using a DependencyProperty as the backing store for CurrentEngineer. This enables animation, styling, binding, etc...
		public static readonly DependencyProperty EngineerProperty = DependencyProperty.Register("Engineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));
		public static readonly DependencyProperty TasksListProperty = DependencyProperty.Register("TasksList", typeof(IEnumerable<BO.TaskInList>), typeof(EngineerWindow), new PropertyMetadata(null));


		public EngineerWindow(int id)
		{
			InitializeComponent();

			Engineer = s_bl.Engineer.Read(id);
			TasksList = s_bl.Task.ReadAll(item => item.Complexity <= Engineer.Level);
		}

		private void UpdateTask(object sender, MouseButtonEventArgs e)
		{
			MessageBox.Show("Update Task - not impleted");
		}
	}
}
