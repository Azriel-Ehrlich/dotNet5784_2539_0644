using System;
using System.Windows;
using System.Windows.Threading;

namespace PL
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

		public DateTime CurrentTime
		{
			get { return (DateTime)GetValue(CurrentTimeProperty); }
			set { SetValue(CurrentTimeProperty, value); }
		}

		public static readonly DependencyProperty CurrentTimeProperty = DependencyProperty.Register(
			"CurrentTime", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(null));


		public MainWindow()
		{
			InitializeComponent();
			s_bl.InitClock();
			StartClock();
			UpdateClock();
			// Stop the timer when the window is closed and wait for the thread to stop
			// TODO: save the clock time to the database and load it when the program starts
			this.Closing += (s, e) => { StopClock(); Thread.Sleep(1000); };
		}

		private void UpdateClock() { Dispatcher.Invoke(() => CurrentTime = s_bl.Clock); }
		private void StartClock() { s_bl.StartClockThread(UpdateClock); }
		private void StopClock() { s_bl.StopClockThread(); }

		private void ManagerMenu(object sender, RoutedEventArgs e) { new Manager.ManagerWindow().Show(); }

		private void EngineerMenu(object sender, RoutedEventArgs e) { new Engineer.SelectEngineerWindow().ShowDialog(); }

		private void ClockIncHour(object sender, RoutedEventArgs e) { StopClock(); s_bl.AddHours(1); UpdateClock(); }
		private void ClockDecHour(object sender, RoutedEventArgs e) { StopClock(); s_bl.AddHours(-1); UpdateClock(); }
		private void ClockIncDay(object sender, RoutedEventArgs e) { StopClock(); s_bl.AddDays(1); UpdateClock(); }
		private void ClockDecDay(object sender, RoutedEventArgs e) { StopClock(); s_bl.AddDays(-1); UpdateClock(); }

		private void StartClock(object sender, RoutedEventArgs e) { StartClock(); }
		private void StopClock(object sender, RoutedEventArgs e) { StopClock(); }
	}
}
