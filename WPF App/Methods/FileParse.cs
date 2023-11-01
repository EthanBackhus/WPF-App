using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using WPF_App.Interfaces;

namespace WPF_App.Methods
{
    public class FileParse : IFileParse
    {
        private ConcurrentDictionary<string, int> _words;

        
        public FileParse()
        {
            _words = new Dictionary<string, int>();
        }
        

        public async Task<Dictionary<string, int>> ParseFileAsync(string fileContent, IProgress<int> progress, CancellationToken cancellationToken)
        {
            var _words = new Dictionary<string, int>();

            var updatedFileContent = RemoveAllNewlines(fileContent);
            string[] parsedStrings = updatedFileContent.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            //foreach (var str in parsedStrings)
            // parsedStrings[i] = str
            for(int i = 0; i < parsedStrings.Length; i++)
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

                int progressPercentage = (i * 100) / parsedStrings.Length;
                progress.Report(progressPercentage);
            }

            SortWordsAsync();

            return _words;
        }

        private static string RemoveAllNewlines(string str)
        {
            return Regex.Replace(str, @"\t|\n|\r", " ");
        }

        
        private async Task SortWordsAsync()
        {
            _words = _words.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }
        

    }
}

//                    _words.AddOrUpdate(parsedStrings[i], 1, (key, value) => value + 1);
// 