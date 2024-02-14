using System.Windows;

namespace PL;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	BlApi.IBl s_bl = BlApi.Factory.Get();


    public MainWindow()
    {
        InitializeComponent();
    }

    private void InintalData(object sender, RoutedEventArgs e)
    {
        var ans = MessageBox.Show("Are you sure you want to initial the data?", "Note", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (ans == MessageBoxResult.Yes)
        {
            s_bl.InitializeDB();
            MessageBox.Show("Data has been initialized successfully");
        }
    }
    
    private void ResetData(object sender, RoutedEventArgs e)
    {
        var ans = MessageBox.Show("Are you sure you want to reset all data?", "Note", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (ans == MessageBoxResult.Yes)
        {
            s_bl.ResetDB();
            MessageBox.Show("Data has been reset successfully");
        }
    }

	private void ManagerMenu(object sender, RoutedEventArgs e)
    {
        new Manager.ManagerWindow().Show();
    }
    
    private void EngineerMenu(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Not implemented yet");
    }
}