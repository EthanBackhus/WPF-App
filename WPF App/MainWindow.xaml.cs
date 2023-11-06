using Microsoft.Win32;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WPF_App.Interfaces;
using WPF_App.Methods;
namespace WPF_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CancellationTokenSource _cancellationTokenSource;
        private string _selectedFilePath;
        private readonly IFileParser _fileParser;
        private readonly IDisplay _display;
        private readonly IFileReader _fileReader;

        public MainWindow()
        {
            InitializeComponent();
            _fileReader = new FileReader();
            _display = new Display(output);
            _fileParser = new FileParser();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt"; ;

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedFilePath = openFileDialog.FileName;
                var truncatedFilePath = Path.GetFileName(_selectedFilePath);
                selectedFileLabel.Content = $"Selected File: {truncatedFilePath}";
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
                var fileContent= await Task.Run(() => _fileReader.ReadFileAsync(_selectedFilePath, progress, _cancellationTokenSource.Token));
                var sortedWords = await Task.Run(() => _fileParser.ParseFileAsync(fileContent, progress, _cancellationTokenSource.Token));
                await _display.DisplayOutputAsync(sortedWords);

                browseButton.IsEnabled = true;
                startButton.IsEnabled = true;

                progressBar.Value = 0;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        private async void Cancel_Click(object sender, RoutedEventArgs e)
        {
            progressBar.Value = 0;
            _cancellationTokenSource?.Cancel();
        }
    }
}
