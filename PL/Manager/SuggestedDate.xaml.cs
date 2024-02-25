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
    /// Interaction logic for SuggestedDate.xaml
    /// </summary>
    public partial class SuggestedDate : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();


        public DateTime StartProj
        {
            get { return (DateTime)GetValue(StartProjProperty); }
            set { SetValue(StartProjProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartProj.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartProjProperty =
            DependencyProperty.Register("StartProj", typeof(DateTime), typeof(SuggestedDate), new PropertyMetadata(null));



        public SuggestedDate()
        {
            InitializeComponent();
        }

        private void ScheudledDate_Button(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var t in s_bl.Task.ReadAll(t => t.Dependencies is null))
                {
                    s_bl.Task.UpdateScheduledDate(t.Id, StartProj);

                }
                foreach (var t in s_bl.Task.ReadAll(t => t.Dependencies is not null))
                {
                    DateTime? date = s_bl.SuggestedDate(t, StartProj);
                    if (date is not null)
                    {
                        s_bl.Task.UpdateScheduledDate(t.Id, date.Value);
                    }
                }
                s_bl.SaveScheduledDate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Close();
        }
    }
}
