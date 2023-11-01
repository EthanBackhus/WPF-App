using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_App.Interfaces
{
    public interface IFileReader
    {
        Task<string> ReadFileAsync(string filepath);
    }
}
