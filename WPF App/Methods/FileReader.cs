﻿using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WPF_App.Interfaces;

namespace WPF_App.Methods
{
    public class FileReader : IFileReader
    {
        public FileReader()
        {
        }

        public async Task<string> ReadFileAsync(string filePath, IProgress<int> progress, CancellationToken cancellationToken)
        {
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read,
                           FileShare.Read, bufferSize: 4096, useAsync: true))
                {
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        float totalBytes = fileStream.Length;
                        float progressBytesRead = 0;
                        StringBuilder sb = new StringBuilder();
                        char[] buffer = new char[4096];

                        while (!reader.EndOfStream)
                        {
                            int charsRead = await reader.ReadBlockAsync(buffer, 0, buffer.Length);
                            progressBytesRead += charsRead;

                            var progressToReport = ((progressBytesRead / totalBytes) * 100) / 2;
                            progress.Report((int)progressToReport);

                            sb.Append(buffer, 0, charsRead);
                        }
                        var str = sb.ToString();

                        return (str);
                    }
                }
            }
            catch (OperationCanceledException e)
            {
                MessageBox.Show(e.Message);
                progress.Report(0);
                return string.Empty;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}