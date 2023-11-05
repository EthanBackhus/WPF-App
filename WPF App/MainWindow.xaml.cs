﻿using Microsoft.Win32;
using System;
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
        private readonly IFileParse _fileParser;
        private readonly IDisplay _display;
        private readonly IFileReader _fileReader;

        public MainWindow()
        {
            InitializeComponent();
            _fileReader = new FileReader();
            _display = new Display(output);
            _fileParser = new FileParse();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

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
                var fileContent= await Task.Run(() => _fileReader.ReadFileAsync(_selectedFilePath, progress, _cancellationTokenSource.Token));
                var sortedWords = await Task.Run(() => _fileParser.ParseFileAsync(fileContent, progress, _cancellationTokenSource.Token));
                await _display.DisplayOutput(sortedWords);

                browseButton.IsEnabled = true;
                startButton.IsEnabled = true;
            }
            catch (OperationCanceledException exception)
            {
                progressBar.Value = 0;
            }
        }

        private async void Cancel_Click(object sender, RoutedEventArgs e)
        {
            progressBar.Value = 0;
            _cancellationTokenSource?.Cancel();
        }
    }
}
