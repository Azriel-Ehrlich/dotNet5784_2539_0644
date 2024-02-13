using System.Windows;

namespace PL;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ListOfEngineers(object sender, RoutedEventArgs e)
    {
    }

    private void InintalData(object sender, RoutedEventArgs e)
    {
        var ans = MessageBox.Show("Are you sure you want to initial the data?", "Note", MessageBoxButton.YesNo);

        if (ans == MessageBoxResult.Yes)
        {
            DalTest.Initialization.Do();
            MessageBox.Show("Data has been initialized successfully");
        }
    }
    
    private void ResetData(object sender, RoutedEventArgs e)
    {
        var ans = MessageBox.Show("Are you sure you want to reset all data?", "Note", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (ans == MessageBoxResult.Yes)
        {
            DalTest.Initialization.Reset();
            MessageBox.Show("Data has been reset successfully");
        }
    }

	private void ManagerMenu(object sender, RoutedEventArgs e)
    {
        //MessageBox.Show("Not implemented yet");
        new Manager.ManagerWindow().Show();
    }
    
    private void EngineerMenu(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Not implemented yet");
    }

}