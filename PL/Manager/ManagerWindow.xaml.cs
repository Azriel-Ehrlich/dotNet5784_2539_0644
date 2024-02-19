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

namespace PL.Manager
{
    /// <summary>
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
		BlApi.IBl s_bl = BlApi.Factory.Get();


		public ManagerWindow()
        {
            InitializeComponent();
        }

		private void ShowEngineersList(object sender, RoutedEventArgs e)
		{
			new Engineer.EngineerListWindow().Show();
		}

		private void ShowTasksList(object sender, RoutedEventArgs e)
		{
            new Task.TasksListWindow().Show();
		}

		private void InintalData(object sender, RoutedEventArgs e)
		{
			var ans = MessageBox.Show("Are you sure you want to initial the data?", "Note", MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (ans == MessageBoxResult.Yes)
			{
				s_bl.InitializeDB();
				MessageBox.Show("Data has been initialized successfully");
			}
		}

		private void ResetData(object sender, RoutedEventArgs e)
		{
			var ans = MessageBox.Show("Are you sure you want to reset all data?", "Note", MessageBoxButton.YesNo, MessageBoxImage.Warning);

			if (ans == MessageBoxResult.Yes)
			{
				s_bl.ResetDB();
				MessageBox.Show("Data has been reset successfully");
			}
		}
	
		private void SuggestedDate(object sender, RoutedEventArgs e)
		{
			var ans = MessageBox.Show("Are you sure?", "Note - SuggestedDate", MessageBoxButton.YesNo, MessageBoxImage.Warning);

			if (ans == MessageBoxResult.Yes)
			{
			//	s_bl.SuggestedDate();
				MessageBox.Show("TODO: SuggestedDate does not impleted");
			}
		}
	}
}
