using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Engineer
{
    /// <summary> Interaction logic for EngineerList.xaml </summary>
    public partial class EngineerListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public EngineerListWindow()
        {
            InitializeComponent();
            EngineerList = (s_bl?.Engineer.ReadAll()!).OrderBy(e => e.Id); // sort by ID so it will be easier to find the engineer in the list as a human
        }

        /// <summary> for the list of engineers in the window  </summary>
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

        public BO.EngineerExperienceWithAll LevelCategory { get; set; } = BO.EngineerExperienceWithAll.All;

        private void ChangeToSelectedLevel(object sender, SelectionChangedEventArgs e)
        {
            EngineerList = (LevelCategory == BO.EngineerExperienceWithAll.All) ? s_bl?.Engineer.ReadAll()!
                : s_bl?.Engineer.ReadAll(item => item.Level == (BO.EngineerExperience)LevelCategory)!;
        }

        private void AddEngineer(object sender, RoutedEventArgs e)
        {
            new EngineerWindow().ShowDialog();
        }

        private void UpdateEngineer(object sender, MouseButtonEventArgs e)
        {
            BO.Engineer? eng = (sender as ListView)?.SelectedItem as BO.Engineer;
            if (eng != null)
            {
                new EngineerWindow(eng.Id).ShowDialog();
                EngineerList = s_bl?.Engineer.ReadAll()!;
            }
        }
    }
}
