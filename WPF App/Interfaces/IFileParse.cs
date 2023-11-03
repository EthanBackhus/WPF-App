using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WPF_App.Interfaces
{
    public interface IFileParse
    {
        Task<Dictionary<string, int>> ParseFileAsync(string fileContent, IProgress<int> progress, CancellationToken cancellationToken);
    }
}
