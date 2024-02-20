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
	/// <summary> Select engineer (by ID) and open EngineerWindow </summary>
	public partial class SelectEngineerWindow : Window
	{
		public int EngineerId
		{
			get { return (int)GetValue(IdProperty); }
			set { SetValue(IdProperty, value); }
		}

		public static readonly DependencyProperty IdProperty = DependencyProperty.Register(
				"Clock", typeof(int), typeof(SelectEngineerWindow), new PropertyMetadata(null));

		public SelectEngineerWindow()
		{
			InitializeComponent();
			this.WindowStartupLocation = WindowStartupLocation.CenterScreen; // set position to center of screen
		}

		private void Select(object sender, RoutedEventArgs e)
		{
			try
			{
				EngineerId = 248845367; // TODO: just for testing !!!
				BlApi.Factory.Get().Engineer.Read(EngineerId); // check if exists
				new Engineer.EngineerWindow(EngineerId).Show();
				this.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}
