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

        public async Task<string> ReadFileAsync(string filePath, IProgress<int> progress)
        {
            try
            {
                await using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true))
                {
                    // Code that uses the file stream
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        string text = await reader.ReadToEndAsync();
                        double percentRead = reader.BaseStream.Position / fileStream.Length;
                        //progress.Report((int)percentRead);
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

/*
await using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true))
   {
   // Code that uses the file stream
   using (StreamReader reader = new StreamReader(fileStream))
   {
   string text = await reader.ReadToEndAsync();
   double percentRead = reader.BaseStream.Position / fileStream.Length;
   progress.Report((int) percentRead);
   return text;
   }
   }


using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true))
   {
   using (StreamReader reader = new StreamReader(fileStream))
   {
   char[] buffer = new char[4096];
   long totalBytesRead = 0;
   long fileLength = fileStream.Length;
   StringBuilder fileContents = new StringBuilder();
   
   while (!reader.EndOfStream)
   {
   int bytesRead = await reader.ReadAsync(buffer, 0, buffer.Length);
   totalBytesRead += bytesRead;
   double percentRead = (double)totalBytesRead / fileLength * 100;
   //progress.Report((int)percentRead);
   
   fileContents.Append(buffer, 0, bytesRead);
   }
   
   // At this point, you have read the entire file
   progress.Report(100);
   
   return (fileContents.ToString());
   }
   }
   
 */

