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
	/// <summary> Interaction logic for EngineerWindow.xaml </summary>
	public partial class EngineerWindow : Window
	{

		static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

		public class CurrentEngineerType // for binding
		{
			public BO.Engineer Engineer { get; set; }
			public IEnumerable<BO.TaskInList> TasksList { get; set; }

			public CurrentEngineerType(int id)
			{
				Engineer = s_bl.Engineer.Read(id);
				TasksList = s_bl.Task.ReadAll(item => item.Complexity >= Engineer.Level);
			}
		}

		public CurrentEngineerType CurrentEngineer
		{
			get { return (CurrentEngineerType)GetValue(CurrentEngineerProperty); }
			set { SetValue(CurrentEngineerProperty, value); }
		}

		// Using a DependencyProperty as the backing store for CurrentEngineer. This enables animation, styling, binding, etc...
		public static readonly DependencyProperty CurrentEngineerProperty = DependencyProperty.Register("CurrentEngineer", typeof(CurrentEngineerType), typeof(EngineerDataInputWindow), new PropertyMetadata(null));


		public EngineerWindow()
		{
			InitializeComponent();
			CurrentEngineer = new(248845367);
		}

		private void UpdateTask(object sender, MouseButtonEventArgs e)
		{
			MessageBox.Show("Update Task - not impleted");
		}
	}
}

/*
add list at end 


<Grid Background="DarkCyan">
                <Grid.RowDefinitions>
                    <RowDefinition Height="40*"/>
                    <RowDefinition Height="500*"/>
                    <RowDefinition Height="Auto"/>
                </Grid.RowDefinitions>
                <!--Internal, Upper Grid-->
                <Grid HorizontalAlignment="Stretch" Height="auto" Grid.Row="0" VerticalAlignment="Stretch" Width="auto">
                    <Grid.ColumnDefinitions>
                        <ColumnDefinition Width="*"/>
                        <ColumnDefinition Width="*"/>
                    </Grid.ColumnDefinitions>
                    <Label Content="Select New Task" HorizontalAlignment="Left" Margin="10,10,0,0" Grid.Column="0" FontSize="20" Height="40"/>
                </Grid>
                <!---the list of elements-->
                <ListView Background="PaleTurquoise" Margin="10" Grid.Row="1" ItemsSource="{Binding Path=TasksList}" MouseDoubleClick="UpdateTask">
                    <ListView.View>
                        <GridView>
                            <GridViewColumn/>
                        </GridView>
                    </ListView.View>
                </ListView>

            </Grid>
 
 
 
 
 */
