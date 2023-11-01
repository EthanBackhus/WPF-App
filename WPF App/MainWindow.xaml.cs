using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using WPF_App.Interfaces;
using WPF_App.Methods;

namespace WPF_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IFileProcessor _fileProcessor;
        private CancellationTokenSource _cancellationTokenSource;
        private string _selectedFilePath;
        private IFileParse _fileParser;
        private IDisplay _display;
        private IFileReader _fileReader;
        private TextBox _rawTextBox, _outputTextBox;

        public MainWindow()
        {
            InitializeComponent();
            _rawTextBox = rawTextBox;
            _outputTextBox = outputTextBox;
            _fileReader = new FileReader();
            _display = new Display(_rawTextBox, _outputTextBox);
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
                IProgress<int> progress = new Progress<int>(x => progressBar.Value = x);
                await _fileProcessor.ProcessFileAsync(_selectedFilePath, progress, _cancellationTokenSource.Token);

                browseButton.IsEnabled = true;
                startButton.IsEnabled = true;

                //clear progress bar
                progressBar.Value = 0;
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
            _cancellationTokenSource?.Cancel();
        }
    }
}
