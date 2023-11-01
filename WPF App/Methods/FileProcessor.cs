using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPF_App.Interfaces;

namespace WPF_App.Methods
{
    public class FileProcessor : IFileProcessor
    {
        private IFileParse _parseFile;
        private IDisplay _display;
        private IFileReader _fileReader;

        public FileProcessor(IFileParse parseFile, IDisplay display, IFileReader fileReader)
        {
            _parseFile = parseFile;
            _display = display;
            _fileReader = fileReader;
        }

        public async Task ProcessFileAsync(string filePath, IProgress<int> progress, CancellationToken cancellationToken)
        {
            var fileContent = await _fileReader.ReadFileAsync(filePath);
            await _display.DisplayRawText(fileContent);
            var parsedText = await _parseFile.ParseFileAsync(fileContent, progress, cancellationToken);
            await _display.DisplayOutputText(parsedText);
        }


    }
}
