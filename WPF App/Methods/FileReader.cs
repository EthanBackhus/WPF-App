using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Printing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPF_App.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace WPF_App.Methods
{
    public class FileReader : IFileReader
    {
        private string filePath;
        private IDisplay _display;

        public FileReader()
        {
        }

        public async Task<string> ReadFileAsync(string filePath, IProgress<int> progress, CancellationToken cancellationToken)
        {
            try
            {
                await using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read,
                                 FileShare.Read, bufferSize: 4096, useAsync: true))
                {
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        string text = await reader.ReadToEndAsync();
                        double percentRead = reader.BaseStream.Position / fileStream.Length;
                        //progress.Report((int)percentRead);
                        return text;
                    }
                }

            }
            catch (OperationCanceledException e)
            {
                MessageBox.Show("Operation Cancelled.");
                return string.Empty;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message); // file not found?
                throw new FileNotFoundException();
            }
        }
    }
}

