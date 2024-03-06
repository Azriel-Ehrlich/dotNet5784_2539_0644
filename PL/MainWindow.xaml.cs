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

            var ans = MessageBox.Show("Do you want to load the last clock?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (ans == MessageBoxResult.Yes)
                s_bl.Clock.LoadClock();
            else
                s_bl.Clock.InitClock();

            StartClock();
            UpdateClock();
            // Stop the timer when the window is closed and wait for the thread to stop
            this.Closing += (s, e) =>
            {
                StopClock(); Thread.Sleep(1000);
                s_bl.Clock.SaveClock();
            };
        }

        private void UpdateClock() { Dispatcher.Invoke(() => CurrentTime = s_bl.Clock.CurrentTime); }
        private void StartClock() { s_bl.Clock.StartClockThread(UpdateClock); }
        private void StopClock() { s_bl.Clock.StopClockThread(); }

        private void ManagerMenu(object sender, RoutedEventArgs e) { new Manager.ManagerWindow().Show(); }

        private void EngineerMenu(object sender, RoutedEventArgs e) { new Engineer.SelectEngineerWindow().ShowDialog(); }

        private void ClockIncHour(object sender, RoutedEventArgs e) { StopClock(); s_bl.Clock.AddHours(1); UpdateClock(); }
        private void ClockDecHour(object sender, RoutedEventArgs e) { StopClock(); s_bl.Clock.AddHours(-1); UpdateClock(); }
        private void ClockIncDay(object sender, RoutedEventArgs e) { StopClock(); s_bl.Clock.AddDays(1); UpdateClock(); }
        private void ClockDecDay(object sender, RoutedEventArgs e) { StopClock(); s_bl.Clock.AddDays(-1); UpdateClock(); }

        private void StartClock(object sender, RoutedEventArgs e) { StartClock(); }
        private void StopClock(object sender, RoutedEventArgs e) { StopClock(); }
    }
}
