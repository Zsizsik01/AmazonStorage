using RuntimeTerror.Model;
using RuntimeTerror.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace RuntimeTerror
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private StorageViewModel _viewModel;
        private MainWindow _view;
        private Controller _model;
        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }
        private void App_Startup(object sender, StartupEventArgs e)
        {
            _model = new Controller();
            _viewModel = new StorageViewModel(_model);

            _viewModel.Preview += new EventHandler(ViewModel_Preview);
            _viewModel.EnergyModify += new EventHandler(ViewModel_EnergyModify);
            _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);
            _viewModel.Selected += new EventHandler(ViewModel_Select);
            _viewModel.Deselected += new EventHandler(ViewModel_Deselect);
            _viewModel.BackToTheEditStage += new EventHandler(ViewModel_BackToTheEditStage);
            _viewModel.Finalization += new EventHandler(ViewModel_Finalization);
            _viewModel.Simulate += new EventHandler(ViewModel_Simulate);
            _viewModel.SimulationEnd += new EventHandler(ViewModel_SimulationEnd);

            _view = new MainWindow();
            _view.DataContext = _viewModel;
            _view.Show();
        }

        private void ViewModel_SimulationEnd(object sender, EventArgs e)
        {
            _view.NewSimulation.IsEnabled = true;
            _view.Load.IsEnabled = true;
            _view.Save.IsEnabled = true;
        }

        private void ViewModel_Simulate(object sender, EventArgs e)
        {
            _view.NewSimulation.IsEnabled = false;
            _view.Load.IsEnabled = false;
            _view.Save.IsEnabled = false;
        }

        private void ViewModel_Finalization(object sender, EventArgs e)
        {
            _viewModel.Select = false;
            foreach (StorageField field in _viewModel.Fields)
            {
                field.Selected = false;
            }
            _viewModel.RefreshTable();
        }

        private void ViewModel_BackToTheEditStage(object sender, EventArgs e)
        {
            _view.Preview.IsEnabled = false;
            _view.Preview.Foreground = Brushes.Gray;
            _view.Height.IsEnabled = false;
            _view.Width.IsEnabled = false;

            _view.Shelf.IsEnabled = true;
            _view.Shelf.Foreground = Brushes.White;
            _view.Destination.IsEnabled = true;
            _view.Destination.Foreground = Brushes.White;
            _view.Dock.IsEnabled = true;
            _view.Dock.Foreground = Brushes.White;
            _view.Robot.IsEnabled = true;
            _view.Robot.Foreground = Brushes.White;
            _view.Select.IsEnabled = true;
            _view.Select.Foreground = Brushes.White;
            _view.Deselect.IsEnabled = true;
            _view.Deselect.Foreground = Brushes.White;
            _view.Move.IsEnabled = true;
            _view.Move.Foreground = Brushes.White;
            _view.Delete.IsEnabled = true;
            _view.Delete.Foreground = Brushes.White;
            _view.Finalization.IsEnabled = true;
            _view.Finalization.Foreground = Brushes.White;
            _view.Place.IsEnabled = false;
            _view.Place.Foreground = Brushes.Gray;
            _view.ItemNum.IsEnabled = false;
            _view.EnergyNum.IsEnabled = false;
            _view.Modify.IsEnabled = false;
            _view.Modify.Foreground = Brushes.Gray;
            _view.Speed.IsEnabled = false;
            _view.Speed.Foreground = Brushes.Gray;
            _view.Simulation.IsEnabled = false;
            _view.Simulation.Foreground = Brushes.Gray;
            _view.GetData.IsEnabled = false;
            _view.GetData.Foreground = Brushes.Gray;
        }
        

        private void ViewModel_Deselect(object sender, EventArgs e)
        {
            _viewModel.Select = false;
            _view.Place.IsEnabled = false;
            _view.Place.Foreground = Brushes.White;
            _view.ItemNum.IsEnabled = false;
            foreach (StorageField field in _viewModel.Fields)
            {
                field.Selected = false;
            }
            _viewModel.RefreshTable();
        }

        private void ViewModel_Select(object sender, EventArgs e)
        {
            _viewModel.Select = true;
            _viewModel.RefreshTable();
            _view.Place.IsEnabled = true;
            _view.Place.Foreground = Brushes.White;
            _view.ItemNum.IsEnabled = true;
        }

        private void ViewModel_EnergyModify(object sender, EventArgs e)
        {
            _model.MaxEnergy = _viewModel.MaxEnergy;

            for(int i = 0; i < _model.Robots.Count; i++)
            {
                _model.Robots[i].Energy = _model.MaxEnergy;
            }
        }

        private void ViewModel_ExitGame(object sender, EventArgs e)
        {
            _view.Close();
        }

        private void ViewModel_Preview(object sender, EventArgs e)
        {
            _model.Init();
            _viewModel.FirstSize = _viewModel.PreFirstSize;
            _viewModel.SecondSize = _viewModel.PreSecondSize;
            _model.Storage.GenerateFields();
            _viewModel.SetupTable();
            _viewModel.RefreshTable();
        }
    }
}
