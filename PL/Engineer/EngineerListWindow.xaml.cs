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
            UpdateEngineerList(); //EngineerList = s_bl?.Engineer.ReadAll()!;
        }

        /// <summary> for the list of engineers in the window </summary>
        public IEnumerable<BO.Engineer> EngineersList
        {
            get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }// return the value of the property
            set { SetValue(EngineerListProperty, value); }// set the value of the property
        }

        // Using a DependencyProperty as the backing store for EngineerList. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineersList",
                typeof(IEnumerable<BO.Engineer>),
                typeof(EngineerListWindow), new PropertyMetadata(null));

        public EngineerExperienceWithAllAndDeleted LevelCategory { get; set; } = EngineerExperienceWithAllAndDeleted.All;

        /// <summary> update the list of engineers in the window according to the selected level </summary>
        void UpdateEngineerList()
        {
            EngineersList = (
                (LevelCategory == EngineerExperienceWithAllAndDeleted.All) ? s_bl?.Engineer.ReadAll(e => e.IsActive)!
                : (LevelCategory == EngineerExperienceWithAllAndDeleted.Deleted) ? s_bl?.Engineer.ReadAll(e => !e.IsActive)!
               : s_bl?.Engineer.ReadAll(e => e.IsActive && e.Level == (BO.EngineerExperience)LevelCategory)!
               ).OrderBy(e => e.Id); // sort by ID so it will be easier to find the engineer in the list as a human
        }

        private void ChangeToSelectedLevel(object sender, SelectionChangedEventArgs e)
        {
            UpdateEngineerList();
        }

        private void AddEngineer(object sender, RoutedEventArgs e)
        {
            new EngineerDataInputWindow().ShowDialog();
            UpdateEngineerList();
        }

        private void UpdateEngineer(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView)?.SelectedItem is BO.Engineer eng)
            {
                new EngineerDataInputWindow(eng.Id).ShowDialog();
                UpdateEngineerList();
            }
        }
    }
}
