using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WPF_App.Interfaces
{
    public interface IFileParser
    {
        Task<Dictionary<string, int>> ParseFileAsync(string fileContent, IProgress<int> progress, CancellationToken cancellationToken);
    }
}
