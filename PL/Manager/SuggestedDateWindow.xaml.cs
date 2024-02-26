using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Manager
{
	/// <summary>
	/// Interaction logic for SuggestedDateWindow.xaml
	/// </summary>
	public partial class SuggestedDateWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();


        public DateTime StartProj
        {
            get { return (DateTime)GetValue(StartProjProperty); }
            set { SetValue(StartProjProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartProj.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartProjProperty =
            DependencyProperty.Register("StartProj", typeof(DateTime), typeof(SuggestedDateWindow), new PropertyMetadata(null));


        public SuggestedDateWindow()
        {
            InitializeComponent();
            StartProj = s_bl.Clock;
		}

        private void ScheudledDate_Button(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.MakeSuggestedDates(StartProj);
                s_bl.SaveScheduledDate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            MessageBox.Show("Suggested date added successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}
