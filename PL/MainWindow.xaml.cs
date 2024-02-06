using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
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
            new Engineer.EngineerListWindow().Show();
        }

        private void InintalData(object sender, RoutedEventArgs e)
        {
            var ans = MessageBox.Show("are you sure you want to initial the data?","Note",MessageBoxButton.YesNo);

            if(ans == MessageBoxResult.Yes)
            {
                DalTest.Initialization.Do();
                MessageBox.Show("Data has been initialized");
            }
        }
    }
}