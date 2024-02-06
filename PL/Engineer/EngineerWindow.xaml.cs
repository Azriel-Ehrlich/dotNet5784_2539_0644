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
                if (CurrentEngineer.Id == 0)
                {
                    s_bl.Engineer.Create(CurrentEngineer);
                    MessageBox.Show("Engineer added successfully");
                    this.Close();
                }
                else
                {
                    s_bl.Engineer.Update(CurrentEngineer);
                    MessageBox.Show("Engineer updated successfully");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
