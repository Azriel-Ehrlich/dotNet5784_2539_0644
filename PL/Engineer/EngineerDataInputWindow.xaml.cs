using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public partial class EngineerDataInputWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        readonly bool isNewEngineer; // save the state: create new or update engineer

        public BO.Engineer CurrentEngineer
        {
            get { return (BO.Engineer)GetValue(CurrentEngineerProperty); }
            set { SetValue(CurrentEngineerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentEngineer. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentEngineerProperty = DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(EngineerDataInputWindow), new PropertyMetadata(null));


        public EngineerDataInputWindow(int id = -1)
        {
            InitializeComponent();
            isNewEngineer = (id == ConstantValues.NO_ID);
            CurrentEngineer = isNewEngineer ? new BO.Engineer() { Id = ConstantValues.NO_ID, Email = "", Name = "" } : s_bl.Engineer.Read(id);
        }

        private void AddOrUpdateEngineer(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isNewEngineer)
                {
                    s_bl.Engineer.Create(CurrentEngineer);
                    MessageBox.Show("Engineer added successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    s_bl.Engineer.Update(CurrentEngineer);
                    MessageBox.Show("Engineer updated successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                this.Close(); // close window after adding or updating the engineer
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChangeEngineerkState(object sender, RoutedEventArgs e)
        {
            try
            {
                string text = CurrentEngineer.IsActive ? "delete" : "restore";

                var result = MessageBox.Show($"Are you sure you want to {text} this engineer?", $"{text} engineer", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                    return;

                if (CurrentEngineer.IsActive)
                    s_bl.Engineer.Delete(CurrentEngineer.Id);
                else
                    s_bl.Engineer.Restore(CurrentEngineer.Id);

                MessageBox.Show($"Engineer {text}d successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close(); // close window after deleting/restoring the Task
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

		/// <summary> Event handler for validating text input in a TextBox to ensure only double values are entered. </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">TextCompositionEventArgs containing information about the text composition.</param>
		private void DoubleValidationTextBox(object sender, TextCompositionEventArgs e)
            => e.Handled = !double.TryParse((sender as TextBox)!.Text + e.Text, out _);
    }
}
