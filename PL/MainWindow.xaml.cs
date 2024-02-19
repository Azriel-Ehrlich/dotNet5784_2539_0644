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
	}

	public ClockType Clock
	{
		get { return (ClockType)GetValue(ClockProperty); }
		set { SetValue(ClockProperty, value); }
	}

	// Using a DependencyProperty as the backing store for CurrentEngineer. This enables animation, styling, binding, etc...
	public static readonly DependencyProperty ClockProperty =
		DependencyProperty.Register("Clock",
			typeof(ClockType), typeof(MainWindow),
			new PropertyMetadata(null));


	void UpdateClock()
	{
		Clock = new() { Clock = s_bl.Clock };
	}

	public MainWindow()
	{
		InitializeComponent();
		s_bl.InitClock();
		UpdateClock();
	}

	private void ManagerMenu(object sender, RoutedEventArgs e)
	{
		new Manager.ManagerWindow().Show();
	}

	private void EngineerMenu(object sender, RoutedEventArgs e)
	{
		new Engineer.EngineerWindow().Show();
	}

	private void ClockIncHour(object sender, RoutedEventArgs e) { s_bl.AddHours(1); UpdateClock(); }
	private void ClockDecHour(object sender, RoutedEventArgs e) { s_bl.AddHours(-1); UpdateClock(); }
	private void ClockIncDay(object sender, RoutedEventArgs e) { s_bl.AddDays(1); UpdateClock(); }
	private void ClockDecDay(object sender, RoutedEventArgs e) { s_bl.AddDays(-1); UpdateClock(); }
}