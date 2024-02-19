using System.Windows;
using System.Windows.Controls;

namespace PL.Engineer
{
	/// <summary>
	/// Interaction logic for EngineerWindow.xaml
	/// </summary>
	public partial class EngineerDataInputWindow : Window
	{
		static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

		public class CurrentEngineerType // for binding
		{
			public BO.Engineer Engineer { get; set; }
			public bool isNewEngineer { set; get; } // save the state: create new or update engineer

			public CurrentEngineerType(int id)
			{
				isNewEngineer = (id == -1);
				Engineer = isNewEngineer ? new BO.Engineer() {Id=-1, Email = "", Name = "" } : s_bl.Engineer.Read(id);
			}
		}

		public CurrentEngineerType CurrentEngineer
		{
			get { return (CurrentEngineerType)GetValue(CurrentEngineerProperty); }
			set { SetValue(CurrentEngineerProperty, value); }
		}

		// Using a DependencyProperty as the backing store for CurrentEngineer. This enables animation, styling, binding, etc...
		public static readonly DependencyProperty CurrentEngineerProperty = DependencyProperty.Register("CurrentEngineer", typeof(CurrentEngineerType), typeof(EngineerDataInputWindow), new PropertyMetadata(null));


		public EngineerDataInputWindow(int Id = -1)
		{
			InitializeComponent();
			CurrentEngineer = new(Id);
		}

		private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (CurrentEngineer.isNewEngineer)
				{
					s_bl.Engineer.Create(CurrentEngineer.Engineer);
					MessageBox.Show("Engineer added successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
				}
				else
				{
					s_bl.Engineer.Update(CurrentEngineer.Engineer);
					MessageBox.Show("Engineer updated successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
				}

				this.Close(); // close window after adding or updating the engineer
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}
