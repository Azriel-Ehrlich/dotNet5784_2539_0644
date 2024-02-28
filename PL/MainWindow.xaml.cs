using System.Windows;

namespace PL;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

	public DateTime CurrentTime
	{
		get { return (DateTime)GetValue(CurrentTimeProperty); }
		set { SetValue(CurrentTimeProperty, value); }
	}

	public static readonly DependencyProperty CurrentTimeProperty = DependencyProperty.Register(
		"CurrentTime", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(null));

	void UpdateTime()
	{
		CurrentTime = s_bl.Clock;
	}

	public MainWindow()
	{
		InitializeComponent();
		s_bl.InitClock();
		UpdateTime();
		new Manager.GantChartWindow().Show();
	}

	private void ManagerMenu(object sender, RoutedEventArgs e)
	{
		new Manager.ManagerWindow().Show();
	}

	private void EngineerMenu(object sender, RoutedEventArgs e)
	{
		new Engineer.SelectEngineerWindow().ShowDialog();
	}

	private void ClockIncHour(object sender, RoutedEventArgs e) { s_bl.AddHours(1); UpdateTime(); }
	private void ClockDecHour(object sender, RoutedEventArgs e) { s_bl.AddHours(-1); UpdateTime(); }
	private void ClockIncDay(object sender, RoutedEventArgs e) { s_bl.AddDays(1); UpdateTime(); }
	private void ClockDecDay(object sender, RoutedEventArgs e) { s_bl.AddDays(-1); UpdateTime(); }
}