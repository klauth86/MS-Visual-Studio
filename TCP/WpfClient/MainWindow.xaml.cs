using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
                ClientHelper.Start();
                ClientHelper.GetGame();
                InitializeGame();
            }
            catch (Exception e) {
                MessageBox.Show(e.Message);
                Application.Current.Shutdown();
            }
        }

        private void InitializeGame()
        {
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
                    emptyButton.Click += OnClickEmpty;
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
                unitButton.Click += OnClickUnit;
                grid1.Children.Add(unitButton);
            }
        }

        private void OnClickUnit(object sender, RoutedEventArgs e)
        {
            if (Selected != null)
            {
                Selected.BorderBrush = Brushes.Gray;
            }
            Selected = sender as Button;
            Selected.BorderBrush = Brushes.Blue;
        }

        private void OnClickEmpty(object sender, RoutedEventArgs e)
        {
            if (Selected != null)
            {
                var turns = ClientHelper.GetTurn((int)Selected.Tag, Grid.GetRow(sender as Button), Grid.GetColumn(sender as Button));
                ProcessTurn(turns);
            }
        }

        private async void ProcessTurn(int[] turns)
        {
            for (int i = 2; i < turns.Length;)
            {
                await Task.Delay(30);
                Grid.SetRow(Selected, turns[i++]);
                Grid.SetColumn(Selected, turns[i++]);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ClientHelper.Stop();
        }
    }
}
