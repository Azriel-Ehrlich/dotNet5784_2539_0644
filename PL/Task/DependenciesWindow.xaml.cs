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
    /// Interaction logic for DependenciesWindow.xaml
    /// This window allow us to add or remove dependencies from a task
    /// </summary>
    public partial class DependenciesWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public int IdTextBox
        {
            get { return (int)GetValue(IdTextBoxProperty); }
            set { SetValue(IdTextBoxProperty, value); }
        }

        public BO.TaskInList CurrentTask
        {
            get { return (BO.TaskInList)GetValue(CurrentTaskProperty); }
            set { SetValue(CurrentTaskProperty, value); }
        }
        
        public IEnumerable<BO.TaskInList> Dependencies
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(DependenciesProperty); }
            set { SetValue(DependenciesProperty, value); }
        }

        public static readonly DependencyProperty IdTextBoxProperty = DependencyProperty.Register("IdTextBox", typeof(int), typeof(DependenciesWindow), new PropertyMetadata(null));
        public static readonly DependencyProperty CurrentTaskProperty = DependencyProperty.Register("CurrentTask", typeof(BO.TaskInList), typeof(DependenciesWindow), new PropertyMetadata(null));
        public static readonly DependencyProperty DependenciesProperty = DependencyProperty.Register("Dependencies", typeof(IEnumerable<BO.TaskInList>), typeof(DependenciesWindow), new PropertyMetadata(null));

        public DependenciesWindow(int taskId)
        {
            InitializeComponent();

            BO.Task task = s_bl.Task.Read(taskId);
            task.Dependencies ??= new();
            Dependencies = task.Dependencies;
            CurrentTask = BO.TaskInList.FromTask(task);
        }

        private void AddDependency(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.Task task = s_bl.Task.Read(CurrentTask.Id);
                if (task.Dependencies is null)
                    task.Dependencies = new();

                // search for the task in the list
                if (task.Dependencies.Any(t => t.Id == IdTextBox))
                    throw new InvalidOperationException("The task is already in the dependencies list");

                // add the task to the dependencies list
                task.Dependencies.Add(BO.TaskInList.FromTask(s_bl.Task.Read(IdTextBox)));

                // update the task
                s_bl.Task.Update(task);

                Dependencies = task.Dependencies;

                MessageBox.Show("dependency added succesfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveDependency(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.Task task = s_bl.Task.Read(CurrentTask.Id);
                if (task.Dependencies is null)
                    task.Dependencies = new();

                // search for the task in the list
                if (!task.Dependencies.Any(t => t.Id == IdTextBox))
                    throw new InvalidOperationException("The task does not exists in the dependencies list");

                // remove the task from the dependencies list
                //task.Dependencies.Remove(BO.TaskInList.FromTask(s_bl.Task.Read(IdTextBox)));
                // use RemoveAt:
                int index = 0;
                foreach (var t in task.Dependencies)
                {
                    if (t.Id == IdTextBox)
                        break;
                    index++;
                }
                task.Dependencies.RemoveAt(index);

                // update the task
                s_bl.Task.Update(task);

                Dependencies = task.Dependencies;

                MessageBox.Show("dependency removed succesfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
