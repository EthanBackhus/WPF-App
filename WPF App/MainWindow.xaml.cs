using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using WPF_App.Interfaces;
using WPF_App.Methods;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace WPF_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IFileProcessor _fileProcessor;
        private CancellationTokenSource _cancellationTokenSource;
        private string _selectedFilePath;
        private readonly IFileParse _fileParser;
        private readonly IDisplay _display;
        private readonly IFileReader _fileReader;

        public ObservableCollection<KeyValuePairViewModel<string, int>> KeyValuePairs { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            _fileReader = new FileReader();
            _display = new Display();
            _fileParser = new FileParse();
            _fileProcessor = new FileProcessor(_fileParser, _display, _fileReader);
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            // maybe change to "Text files (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedFilePath = openFileDialog.FileName;
                selectedFileLabel.Content = $"Selected File: {_selectedFilePath}";
            }
        }

        private async void Start_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedFilePath))
            {
                MessageBox.Show("Please select a file to process first.");
                return;
            }

            browseButton.IsEnabled = false;
            startButton.IsEnabled = false;

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                var progress = new Progress<int>(x => progressBar.Value = x);
                //var fileContent= await Task.Run(() => _fileProcessor.ProcessFileAsync1(_selectedFilePath, progress, _cancellationTokenSource.Token));
                var fileContent= await Task.Run(() => _fileReader.ReadFileAsync(_selectedFilePath, progress, _cancellationTokenSource.Token));
                var sortedWords = await Task.Run(() => _fileProcessor.ProcessFileAsync2(fileContent, progress, _cancellationTokenSource.Token));

                await DisplayOutputText(sortedWords);

                //await _fileProcessor.ProcessFileAsync(_selectedFilePath, progress, _cancellationTokenSource.Token);
                browseButton.IsEnabled = true;
                startButton.IsEnabled = true;

                //clear progress bar
                //progressBar.Value = 0
            }
            catch (Exception exception) // maybe change to operation failed exception
            {
                Console.WriteLine(exception);
                throw;
            }
            finally
            {
                //_cancellationTokenSource?.Dispose();
            }

        }

        private async void Cancel_Click(object sender, RoutedEventArgs e)
        {
            progressBar.Value = 0;
            _cancellationTokenSource?.Cancel();
        }

        public async Task DisplayOutputText(Dictionary<string, int> words)
        {
            DataContext = null;
            KeyValuePairs = null;

            KeyValuePairs = new ObservableCollection<KeyValuePairViewModel<string, int>>(
                words.Select(kv => new KeyValuePairViewModel<string, int> { Key = kv.Key, Value = kv.Value }));

            DataContext = this;
        }
    }

    public class KeyValuePairViewModel<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
    }
}
