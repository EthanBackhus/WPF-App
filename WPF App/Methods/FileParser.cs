using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WPF_App.Interfaces;

namespace WPF_App.Methods
{
    public class FileParser : IFileParser
    {
        private Dictionary<string, int> _words;

        public FileParser()
        {
        }
        
        public async Task<Dictionary<string, int>> ParseFileAsync(string fileContent, IProgress<int> progress, CancellationToken cancellationToken)
        {
            try
            {
                _words = new Dictionary<string, int>();

                var updatedFileContent = RemoveAllNewlines(fileContent);
                string[] parsedStrings = updatedFileContent.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var updateFrequency = parsedStrings.Length / 100;
                int percent = 0;

                for (int i = 0; i < parsedStrings.Length; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (!_words.ContainsKey(parsedStrings[i]))
                    {
                        _words.Add(parsedStrings[i], 1);
                    }
                    else
                    {
                        _words[parsedStrings[i]]++;
                    }

                    if (i % updateFrequency == 0)
                    {
                        percent++;
                        var reportedPercent = (percent / 2) + 50;
                        progress.Report(reportedPercent);
                    }
                }

                SortWordsDescending();

                return _words;
            }
            catch (OperationCanceledException exception)
            {
                MessageBox.Show(exception.Message);
                progress.Report(0);
                return new Dictionary<string, int>();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private static string RemoveAllNewlines(string str)
        {
            return Regex.Replace(str, @"\t|\n|\r", " ");
        }

        private void SortWordsDescending()
        {
            _words = _words.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}