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
            Engineer = new() { Id = id, Email = "", Name = "" };
            UpdateFields();
        }


        /// <summary> update the fields of the window after changes </summary>
        void UpdateFields()
        {
			Engineer = s_bl.Engineer.Read(Engineer.Id);
			TasksList = s_bl.Task.ReadAll(t => t.Engineer == null && t.Complexity <= Engineer.Level);
		}

		private void SelectTask(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView)!.SelectedItem is BO.TaskInList task)
            {
                try
                {
                    s_bl.Task.StartTask(task.Id, Engineer.Id);
                    MessageBox.Show($"{Engineer.Name}'s task update succesfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                UpdateFields();
            }
        }

		private void FinishTask(object sender, RoutedEventArgs e)
		{
            if (Engineer.Task == null)
            {
                MessageBox.Show("First you need to get a new task");
                return;
            }

            var ans = MessageBox.Show("Are you sure you finished your task?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (ans == MessageBoxResult.No)
                return;

            try
            {
                s_bl.Task.FinishTask(Engineer.Task.Id);
			}
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            UpdateFields();
		}
	}
}
