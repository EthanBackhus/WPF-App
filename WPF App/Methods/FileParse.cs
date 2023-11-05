using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using WPF_App.Interfaces;

namespace WPF_App.Methods
{
    public class FileParse : IFileParse
    {
        private Dictionary<string, int> _words;

        public FileParse()
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

                SortWordsAsync();

                return _words;
            }
            catch (OperationCanceledException exception)
            {
                MessageBox.Show(exception.Message);
                progress.Report(0);
                return new Dictionary<string, int>();
            }
            catch (Exception e) // should I have this here?
            {
                throw new Exception(e.Message);
            }
        }

        // should this be static?
        private static string RemoveAllNewlines(string str)
        {
            return Regex.Replace(str, @"\t|\n|\r", " ");
        }

        // should this be async?
        private void SortWordsAsync()
        {
            _words = _words.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}