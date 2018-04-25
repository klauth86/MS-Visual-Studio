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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TcpClientLib;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Button Selected { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                ClientHelper.StartClient();
                ClientHelper.GetGameEngine();
                for (int i = 0; i < ClientHelper.Dimension; i++)
                {
                    grid1.ColumnDefinitions.Add(new ColumnDefinition());
                    grid1.RowDefinitions.Add(new RowDefinition());
                }

                for (int i = 0; i < ClientHelper.Dimension; i++)
                {
                    for (int j = 0; j < ClientHelper.Dimension; j++)
                    {
                        var emptyButton = new Button();
                        Grid.SetRow(emptyButton, i);
                        Grid.SetColumn(emptyButton, j);
                        emptyButton.Click += onClickEmpty;
                        grid1.Children.Add(emptyButton);
                        emptyButton.BorderThickness = new Thickness(5);
                    }
                }
                foreach (var item in ClientHelper.UnitsDict)
                {
                    var unitButton = new Button(); unitButton.Background = Brushes.DarkGoldenrod;
                    unitButton.BorderThickness = new Thickness(5);
                    Grid.SetRow(unitButton, item.Value.X);
                    Grid.SetColumn(unitButton, item.Value.Y);
                    unitButton.Tag = item.Key;
                    unitButton.Click += onClickUnit;
                    grid1.Children.Add(unitButton);
                }


            }
            catch (Exception e)
            {
                var message = e.Message;
            }
        }

        private void onClickUnit(object sender, RoutedEventArgs e)
        {
            if (Selected != null)
            {
                Selected.BorderBrush = Brushes.Gray;
            }
            Selected = sender as Button;
            Selected.BorderBrush = Brushes.Blue;
        }

        private void onClickEmpty(object sender, RoutedEventArgs e)
        {
            if (Selected != null)
            {
                var turns = ClientHelper.GetTurn((int)Selected.Tag, Grid.GetRow(sender as Button), Grid.GetColumn(sender as Button));
                processTurns(turns);
                
            }
        }

        private async void processTurns(int[] turns)
        {
            for (int i = 0; i < turns.Length;)
            {
                await Task.Delay(1000);
                Grid.SetRow(Selected, turns[i++]);
                Grid.SetColumn(Selected, turns[i++]);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ClientHelper.CloseClient();
        }
    }
}
