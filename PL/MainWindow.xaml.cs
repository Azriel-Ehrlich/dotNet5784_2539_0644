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

        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            
           
                s_bl.InitClock();
            

            // Start the timer
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick!;
            timer.Start();

            this.Closing += (sender, e) => timer.Stop();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Increase the time by 1 hour every second
            s_bl.AddMinutes(10);
            s_bl.AddSeconds(1);
            UpdateTime();
        }

        private void UpdateTime()
        {
            CurrentTime = s_bl.Clock;
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
}
