using System.Collections.Generic;
using System.Threading.Tasks;

namespace WPF_App.Interfaces
{
    public interface IDisplay
    {
        Task DisplayOutput(Dictionary<string, int> sortedWords);
    }
}
