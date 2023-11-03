using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WPF_App.Interfaces;

namespace WPF_App.Methods
{
    public class FileProcessor : IFileProcessor
    {
        private readonly IFileParse _parseFile;
        private IDisplay _display;
        private readonly IFileReader _fileReader;

        public FileProcessor(IFileParse parseFile, IDisplay display, IFileReader fileReader)
        {
            _parseFile = parseFile;
            _display = display;
            _fileReader = fileReader;
        }

        public async Task<string> ProcessFileAsync1(string filePath, IProgress<int> progress, CancellationToken cancellationToken)
        { 
             var fileContent = await Task.Run(() => _fileReader.ReadFileAsync(filePath, progress));
             return fileContent;
        }

        public async Task<Dictionary<string, int>> ProcessFileAsync2(string fileContent, IProgress<int> progress, CancellationToken cancellationToken)
        {
            var parsedText = await Task.Run(() => _parseFile.ParseFileAsync(fileContent, progress, cancellationToken));
            return parsedText;
        }
    }
}
