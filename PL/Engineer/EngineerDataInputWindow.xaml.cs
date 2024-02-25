﻿using System.Windows;
using System.Windows.Controls;

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

        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
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
    }
}
