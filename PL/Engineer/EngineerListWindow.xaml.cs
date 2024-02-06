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

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerList.xaml
    /// </summary>
    public partial class EngineerListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public EngineerListWindow()
        {
            InitializeComponent();
            EngineerList = s_bl?.Engineer.ReadAll()!;
        }
        /// <summary>
        ///  for the list of engineers in the window 
        /// </summary>
        public IEnumerable<BO.Engineer> EngineerList
        {
            get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }// return the value of the property
            set { SetValue(EngineerListProperty, value); }// set the value of the property
        }

        // Using a DependencyProperty as the backing store for EngineerList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList", 
                typeof(IEnumerable<BO.Engineer>), 
                typeof(EngineerListWindow), new PropertyMetadata(null));

        public BO.EngineerExperience LevelCategory { get; set; } = BO.EngineerExperience.All;

        private void ChangeToSelectedLevel(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
