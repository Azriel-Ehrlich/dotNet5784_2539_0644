using System.Windows;

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public partial class EngineerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public BO.Engineer CurrentEngineer
        {
            get { return (BO.Engineer)GetValue(CurrentEngineerProperty); }
            set { SetValue(CurrentEngineerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentEngineer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentEngineerProperty = DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));


        public EngineerWindow(int Id = 0)
        {
            InitializeComponent();
            CurrentEngineer = (Id != 0) ? s_bl.Engineer.Read(Id) : new BO.Engineer() { Email = "", Name = "" };
        }

        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (doesEngineerExist(CurrentEngineer.Id))
                {
                    s_bl.Engineer.Update(CurrentEngineer);
                    MessageBox.Show("Engineer updated successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    s_bl.Engineer.Create(CurrentEngineer);
                    MessageBox.Show("Engineer added successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex) when (ex is BO.BlInvalidParameterException || ex is BO.BlDoesNotExistException)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unknown error:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        bool doesEngineerExist(int id)
        {
            try
            {
                s_bl.Engineer.Read(id);
                return true;
            }
            catch (BO.BlDoesNotExistException)
            {
                return false;
            }
        }
    }
}
