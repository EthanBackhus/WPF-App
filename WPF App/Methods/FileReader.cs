using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Printing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPF_App.Interfaces;

namespace WPF_App.Methods
{
    public class FileReader : IFileReader
    {
        private string filePath;

        public FileReader()
        {
        }

        public async Task<string> ReadFileAsync(string filePath)
        {
            try
            {
                await using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true))
                {
                    // Code that uses the file stream
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        string text = await reader.ReadToEndAsync();
                        return text;
                    }
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message); // Change this to log the exception or handle it appropriately
                throw new FileNotFoundException();
            }
        }
    }
}


