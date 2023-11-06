using System;
using System.Threading;
using System.Threading.Tasks;

namespace WPF_App.Interfaces
{
    public interface IFileReader
    {
        Task<string> ReadFileAsync(string filepath, IProgress<int> progress, CancellationToken cancellationToken);
    }
}
