using System.Windows;

namespace PL.Engineer
{
	/// <summary>
	/// Interaction logic for EngineerWindow.xaml
	/// </summary>
	public partial class EngineerWindow : Window
	{
		static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

		public BO.Engineer CurrentEngineer
		{
			get { return (BO.Engineer)GetValue(CurrentEngineerProperty); }
			set { SetValue(CurrentEngineerProperty, value); }
		}

		// Using a DependencyProperty as the backing store for CurrentEngineer.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty CurrentEngineerProperty = DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));

		readonly bool isAdd; // save the state: add or update engineer

		public EngineerWindow(int Id = 0)
		{
			InitializeComponent();
			CurrentEngineer = (Id != 0) ? s_bl.Engineer.Read(Id) : new BO.Engineer() { Email = "", Name = "" };
			isAdd = (Id == 0);
			ConvertIdToBool.CanChangeID = isAdd;
		}

		private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (isAdd)
				{
					s_bl.Engineer.Create(CurrentEngineer);
					MessageBox.Show("Engineer added successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
				}
				else
				{
					s_bl.Engineer.Update(CurrentEngineer);
					MessageBox.Show("Engineer updated successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
				}

				this.Close(); // close window after adding or updating the engineer
			}
			catch (Exception ex) when (
				ex is BO.BlAlreadyExistsException ||
				ex is BO.BlDoesNotExistException ||
				ex is BO.BlInvalidParameterException
				)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Unknown error:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}
