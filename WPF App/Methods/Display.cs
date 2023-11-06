using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPF_App.Interfaces;

namespace WPF_App.Methods
{
    public class Display : IDisplay
    {
        private readonly DataGrid _dataGrid;

        public Display(DataGrid dataGrid)
        {
            _dataGrid = dataGrid;
        }

        public class KeyValuePairViewModel<TKey, TValue>
        {
            public TKey? Word { get; set; }
            public TValue? Count { get; set; }
        }
        

        private ObservableCollection<KeyValuePairViewModel<string, int>> _keyValuePairs;

        public async Task DisplayOutput(Dictionary<string, int> sortedWords)
        {
            _keyValuePairs = new ObservableCollection<KeyValuePairViewModel<string, int>>(
                sortedWords.Select(kv => new KeyValuePairViewModel<string, int> { Word = kv.Key, Count = kv.Value }));

            _dataGrid.ItemsSource = _keyValuePairs;
        }

    }
}
