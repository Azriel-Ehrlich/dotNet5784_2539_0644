using System.Windows;
using System.Windows.Controls;

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public partial class EngineerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public EngineerWindow(int Id=0)
        {
            InitializeComponent();
            if (Id != 0)
            {
                CurrentEngineer = s_bl.Engineer.Read(Id);
            }
             else CurrentEngineer = new BO.Engineer() { Email = "", Name = "" };
        }



        public BO.Engineer CurrentEngineer
        {
            get { return (BO.Engineer)GetValue(CurrentEngineerProperty); }
            set { SetValue(CurrentEngineerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentEngineer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentEngineerProperty =
            DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));

        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Engineer.Read(CurrentEngineer.Id);
                s_bl.Engineer.Update(CurrentEngineer);
                MessageBox.Show("Engineer updated successfully");
            }
            catch (BO.BlDoesNotExistException )
            {
                try
                {
                    s_bl.Engineer.Create(CurrentEngineer);
                    MessageBox.Show("Engineer added successfully");
                }
                catch(BO.BlInvalidParameterException ex1)
                {
                    MessageBox.Show(ex1.Message);
                }
            }
            catch(BO.BlInvalidParameterException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
