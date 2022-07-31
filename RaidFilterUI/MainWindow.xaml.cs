using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using Newtonsoft.Json;
using RaidArtifactsFilter;
using RaidArtifactsFilter.Extensions;
using RaidFilterUI.Forms;
using RaidFilterUI.Helpers;
using Path = System.IO.Path;

namespace RaidFilterUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<ArtifactControl> ArtifactControlPool { get; set; } = new List<ArtifactControl>();

        public int CurrentVisibleArtifactControlCount { get; set; }
        private int _currentCountInRow;
        private FilterService FilterService { get; }

        public MainWindow()
        {
            InitializeComponent();
            FilterService = new FilterService();
            ThreadPool.QueueUserWorkItem(async state => await InitData());

            FilterButton.IsEnabled = false;
            UpdateButton.IsEnabled = false;
            FilterFileComboBox_DropDownOpened(null, null);
            if (FilterFileComboBox.Items.Count > 0) 
                FilterFileComboBox.SelectedIndex = 0;

            for (var i = 0; i < 10; i++)
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition());

            for (var i = 0; i < 1000; i++)
                MainGrid.RowDefinitions.Add(new RowDefinition());
        }

        public async Task InitData()
        {
            await FilterService.Init();

            MainGrid.Dispatcher.Invoke(() =>
            {
                FilterButton.IsEnabled = true;
                UpdateButton.IsEnabled = true;
            });
        }

        public void Filter(string? filePath, bool isKeep)
        {
            TraceService.TimingArrStart();
            TraceService.Instance.TimingSpace();
            TraceService.Instance.TimingSpace();
            TraceService.Instance.TimingSpace();

            if (filePath == null)
            {
                MessageBox.Show("Select the file first");
            }
            try
            {
                FilterService.SetFile($"{filePath}.filter");
                var artifacts = FilterService.GetFilteredItems(isKeep);
                var artifactControlModes = artifacts.Select(artifact => artifact.ToArtifactControlModel()).ToList();
                TraceService.TimingArr("Filter");

                UpdateArtifactControls(artifactControlModes.ToArray());
            }
            catch (Exception e)
            {
                MainGrid.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(e.Message);
                });
            }
        }

        public ArtifactControl[] GetFromThePool(int count)
        {
            TraceService.TimingArrStart();

            var artifactControlCache = ArtifactControlPool;
            if (artifactControlCache.Count < count)
            {
                var newElems = Enumerable
                    .Repeat(0, count - artifactControlCache.Count)
                    .Select(el => new ArtifactControl())
                    .ToArray();

                artifactControlCache.AddRange(newElems);
                TraceService.TimingArr("Elements created");
            }

            for (var i = 0; i < artifactControlCache.Count; i++)
            {
                artifactControlCache[i].Visibility = i < count ? Visibility.Visible : Visibility.Collapsed;
            }

            return artifactControlCache.Take(count).ToArray();
        }
        

        public void UpdateArtifactControls(ArtifactControlModel[] artifactControlModes)
        {
            Dispatcher.Invoke(() =>
            {
                try
                {
                    TraceService.TimingArrStart();
                    CurrentVisibleArtifactControlCount = artifactControlModes.Length;
                    var artifactControls = GetFromThePool(artifactControlModes.Length);

                    for (var i = 0; i < artifactControlModes.Length; i++)
                    {
                        artifactControls[i].Init(artifactControlModes[i]);
                    }

                    TraceService.TimingArr("Elements updated");

                    AddArtifactsToGrid(artifactControls.ToArray(), true);
                    TraceService.TimingArr("Elements added to grid");
                }
                catch (Exception e)
                {
                    MainGrid.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show(e.Message);
                    });
                }

                using var dispatcher = Dispatcher.DisableProcessing();

                Dispatcher.BeginInvoke(new Action(() =>
                {

                }));
            });
        }

        public int GetCountInRow()
        {
            var width = MainGrid.Width;
            if (double.IsNaN(width))
                width = this.Width;

            var artifactControl = new ArtifactControl();
            return (int)(width / (artifactControl.Width + 10));
        }

        public void AddArtifactsToGrid(ArtifactControl[]? items, bool isClear = false)
        {
            if (items == null)
                return;

            ResizeGrid();

            if (MainGrid.Children.Count < ArtifactControlPool.Count)
            {
                MainGrid.Children.Clear();
                foreach (var artifactControl in ArtifactControlPool)
                {
                    MainGrid.Children.Add(artifactControl);
                }
            }
        }

        public void ResizeGrid()
        {
            var countInRow = GetCountInRow();
            _currentCountInRow = countInRow;

            for (var i = 0; i < CurrentVisibleArtifactControlCount; i++)
            {
                var item = ArtifactControlPool[i];
                var row = i / countInRow;
                var column = i % countInRow;

                Grid.SetRow(item, row);
                Grid.SetColumn(item, column);
            }
        }

        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ArtifactControlPool.Count > 0)
            {
                if (_currentCountInRow != GetCountInRow())
                {
                    ResizeGrid();
                }
            }
        }

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(async state => await FilterService.UpdateArtifacts());
           
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            var filePath = FilterFileComboBox.SelectedItem?.ToString();
            var isKeep = InvertCheckBox.IsChecked != null && InvertCheckBox.IsChecked.Value;
            ThreadPool.QueueUserWorkItem(state => Filter(filePath, isKeep));
        }

        private void FilterFileComboBox_DropDownOpened(object sender, EventArgs e)
        {
            FilterFileComboBox.ItemsSource = Directory.GetFiles(
                    Directory.GetCurrentDirectory(),
                    "*.filter")
                .Select(Path.GetFileNameWithoutExtension)
                .ToArray();
        }
    }
}
