using System.Windows;

namespace PL;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

	public class ClockType // for binding
	{
		public DateTime Clock { get; set; }
		public ClockType()
		{
			Clock = s_bl.Clock;
		}
	}

	public ClockType Clock
	{
		get { return (ClockType)GetValue(ClockProperty); }
		set { SetValue(ClockProperty, value); }
	}

	// Using a DependencyProperty as the backing store for CurrentEngineer. This enables animation, styling, binding, etc...
	public static readonly DependencyProperty ClockProperty = DependencyProperty.Register("Clock", typeof(ClockType), typeof(MainWindow), new PropertyMetadata(null));


	public MainWindow()
    {
        InitializeComponent();
		Clock = new();
	}

	private void ManagerMenu(object sender, RoutedEventArgs e)
    {
        new Manager.ManagerWindow().Show();
    }

	private void EngineerMenu(object sender, RoutedEventArgs e)
	{
		new Engineer.EngineerWindow().Show();
	}

	private void ClockAddHour(object sender, RoutedEventArgs e)
	{
		s_bl.AddHours(1);
	}
	private void ClockAddDay(object sender, RoutedEventArgs e)
	{
		s_bl.AddDays(1);
	}
}