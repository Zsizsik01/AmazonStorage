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

namespace RuntimeTerror
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Shelf.IsEnabled = false;
            Shelf.Foreground = Brushes.Gray;
            Destination.IsEnabled = false;
            Destination.Foreground = Brushes.Gray;
            Dock.IsEnabled = false;
            Dock.Foreground = Brushes.Gray;
            Robot.IsEnabled = false;
            Robot.Foreground = Brushes.Gray;
            Select.IsEnabled = false;
            Select.Foreground = Brushes.Gray;
            Deselect.IsEnabled = false;
            Deselect.Foreground = Brushes.Gray;
            Move.IsEnabled = false;
            Move.Foreground = Brushes.Gray;
            Delete.IsEnabled = false;
            Delete.Foreground = Brushes.Gray;
            Finalization.IsEnabled = false;
            Finalization.Foreground = Brushes.Gray;

            Modify.IsEnabled = false;
            Modify.Foreground = Brushes.Gray;
            Place.IsEnabled = false;
            Place.Foreground = Brushes.Gray;

            EnergyNum.IsEnabled = false;
            ItemNum.IsEnabled = false;
            Speed.IsEnabled = false;
            Speed.Foreground = Brushes.Gray;

            Simulation.IsEnabled = false;
            Simulation.Foreground = Brushes.Gray;
            GetData.IsEnabled = false;
            GetData.Foreground = Brushes.Gray;

            Speed.IsEnabled = false;
            Speed.Foreground = Brushes.Gray;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewSimulation_Click(sender, e);
        }

        private void Preview_Click(object sender, RoutedEventArgs e)
        {
            int value;
            if (int.TryParse(Width.Text, out value))
            {
                if (value > 20)
                {
                    MessageBox.Show("Túl nagy a szélesség paramétere!");
                    Width.Text = "20";
                }
                else if (value < 5)
                {
                    MessageBox.Show("Túl kicsi a szélesség paramétere!");
                    Width.Text = "5";
                }
            }
            
            if(int.TryParse(Height.Text, out value))
            {
                if (value > 20)
                {
                    MessageBox.Show("Túl nagy a magasság paramétere!");
                    Height.Text = "20";
                }
                else if (value < 5)
                {
                    MessageBox.Show("Túl kicsi a magasság paramétere!");
                    Height.Text = "5";
                }
            }
            
            if (System.Text.RegularExpressions.Regex.IsMatch(Height.Text, "[^0-9]"))
            {
                MessageBox.Show("A magasság paramétere csak numerikus érték lehet!");
                Height.Text = "5";
            }
            
            if (System.Text.RegularExpressions.Regex.IsMatch(Width.Text, "[^0-9]"))
            {
                MessageBox.Show("A szélesség paramétere csak numerikus érték lehet!");
                Width.Text = "5";
            }

            Preview.IsEnabled = false;
            Preview.Foreground = Brushes.Gray;
            Height.IsEnabled = false;
            Width.IsEnabled = false;

            Shelf.IsEnabled = true;
            Shelf.Foreground = Brushes.White;
            Destination.IsEnabled = true;
            Destination.Foreground = Brushes.White;
            Dock.IsEnabled = true;
            Dock.Foreground = Brushes.White;
            Robot.IsEnabled = true;
            Robot.Foreground = Brushes.White;
            Select.IsEnabled = true;
            Select.Foreground = Brushes.White;
            Deselect.IsEnabled = true;
            Deselect.Foreground = Brushes.White;
            Move.IsEnabled = true;
            Move.Foreground = Brushes.White;
            Delete.IsEnabled = true;
            Delete.Foreground = Brushes.White;
            Finalization.IsEnabled = true;
            Finalization.Foreground = Brushes.White;
        }

        private void NewSimulation_Click(object sender, RoutedEventArgs e)
        {
            Preview.IsEnabled = true;
            Preview.Foreground = Brushes.White;
            Height.IsEnabled = true;
            Width.IsEnabled = true;

            Shelf.IsEnabled = false;
            Shelf.Foreground = Brushes.Gray;
            Destination.IsEnabled = false;
            Destination.Foreground = Brushes.Gray;
            Dock.IsEnabled = false;
            Dock.Foreground = Brushes.Gray;
            Robot.IsEnabled = false;
            Robot.Foreground = Brushes.Gray;
            Select.IsEnabled = false;
            Select.Foreground = Brushes.Gray;
            Deselect.IsEnabled = false;
            Deselect.Foreground = Brushes.Gray;
            Move.IsEnabled = false;
            Move.Foreground = Brushes.Gray;
            Delete.IsEnabled = false;
            Delete.Foreground = Brushes.Gray;
            Finalization.IsEnabled = false;
            Finalization.Foreground = Brushes.Gray;

            Modify.IsEnabled = false;
            Modify.Foreground = Brushes.Gray;
            Place.IsEnabled = false;
            Place.Foreground = Brushes.Gray;

            EnergyNum.IsEnabled = false;
            ItemNum.IsEnabled = false;
            Speed.IsEnabled = false;
            Speed.Foreground = Brushes.Gray;

            Simulation.IsEnabled = false;
            Simulation.Foreground = Brushes.Gray;
            GetData.IsEnabled = false;
            GetData.Foreground = Brushes.Gray;
        }

        private void Finalization_Click(object sender, RoutedEventArgs e)
        {
            Shelf.IsEnabled = false;
            Shelf.Foreground = Brushes.Gray;
            Destination.IsEnabled = false;
            Destination.Foreground = Brushes.Gray;
            Dock.IsEnabled = false;
            Dock.Foreground = Brushes.Gray;
            Robot.IsEnabled = false;
            Robot.Foreground = Brushes.Gray;
            Select.IsEnabled = false;
            Select.Foreground = Brushes.Gray;
            Deselect.IsEnabled = false;
            Deselect.Foreground = Brushes.Gray;
            Move.IsEnabled = false;
            Move.Foreground = Brushes.Gray;
            Delete.IsEnabled = false;
            Delete.Foreground = Brushes.Gray;
            Finalization.IsEnabled = false;
            Finalization.Foreground = Brushes.Gray;

            Modify.IsEnabled = true;
            Modify.Foreground = Brushes.White;
            Place.IsEnabled = true;
            Place.Foreground = Brushes.White;

            EnergyNum.IsEnabled = true;
            ItemNum.IsEnabled = true;

            Simulation.IsEnabled = true;
            Simulation.Foreground = Brushes.White;
            GetData.IsEnabled = true;
            GetData.Foreground = Brushes.White;
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(EnergyNum.Text, "[^0-9]"))
            {
                MessageBox.Show("Az energia paramétere csak numerikus érték lehet!");
                EnergyNum.Text = "100";
            }
        }

        private void Simulation_Click(object sender, RoutedEventArgs e)
        {
            Modify.IsEnabled = false;
            Modify.Foreground = Brushes.Gray;
            EnergyNum.IsEnabled = false;
            Speed.IsEnabled = true;
            Speed.Foreground = Brushes.White;
        }

        private void Place_Click(object sender, RoutedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(ItemNum.Text, "[^0-9]"))
            {
                MessageBox.Show("A termék paramétere csak numerikus érték lehet!");
                ItemNum.Text = "0";
            }
        }
    }
}
