using System.Collections.Generic;
using System.Threading.Tasks;

namespace WPF_App.Interfaces
{
    public interface IDisplay
    {
        Task DisplayOutputAsync(Dictionary<string, int> sortedWords);
    }
}
