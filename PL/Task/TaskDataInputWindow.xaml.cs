﻿using PL.Engineer;
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

        //public bool isNewTask { set; get; } // save the state: create new or update engineer


        public class CurrentTaskType // for binding
        {
            public IEnumerable<BO.TaskInList> AllTasks { get; set; }

            public BO.Task Task { get; set; }

            public CurrentTaskType(int id)
            {
                Task = (id != ConstantValues.NO_ID) ? s_bl.Task.Read(id)
                    : new BO.Task() { Id = id, Alias = "", Description = "" };
                if (Task.Engineer is null)
                {
                    Task.Engineer = new BO.EngineerInTask() { Name = "" };
                }

                AllTasks = s_bl.Task.ReadAll().OrderBy(t => t.Id).Select(t =>
                new TaskInDepList()
                {
                    Id = t.Id,
                    Description = t.Description,
                    Alias = t.Alias,
                    Status = t.Status,
                    IsChecked = Task.Dependencies?.Any(d => d.Id == t.Id) ?? false
                });
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
            //isNewTask = (Id == ConstantValues.NO_ID);
            CurrentTask = new(Id);
        }

        private void AddOrUpdateTask(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CurrentTask.Task.Id == ConstantValues.NO_ID)
                {
                    s_bl.Task.Create(CurrentTask.Task);
                    MessageBox.Show("Task added successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    s_bl.Task.Update(CurrentTask.Task);
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
            BO.TaskInList task = ((sender as CheckBox)!.DataContext as BO.TaskInList)!;
            CurrentTask.Task.Dependencies ??= new();
            CurrentTask.Task.Dependencies.Add(task);
        }
        private void RemoveDependency(object sender, RoutedEventArgs e)
        {
            BO.TaskInList task = ((sender as CheckBox)!.DataContext as BO.TaskInList)!;
            CurrentTask.Task.Dependencies ??= new();
            CurrentTask.Task.Dependencies.Remove(task);
        }

        private void Delete_Task(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Are you sure you want to delete this Task?", "Delete Task", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    return;
                }
                s_bl.Task.Delete(CurrentTask.Task.Id);
                MessageBox.Show("Task deleted successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close(); // close window after deleting the Task
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
     
    }
}
