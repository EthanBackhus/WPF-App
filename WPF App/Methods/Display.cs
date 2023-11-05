using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
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
            public TKey? Key { get; set; }
            public TValue? Value { get; set; }
        }
        

        private ObservableCollection<KeyValuePairViewModel<string, int>> _keyValuePairs;

        public async Task DisplayOutput(Dictionary<string, int> sortedWords)
        {
            _keyValuePairs = new ObservableCollection<Display.KeyValuePairViewModel<string, int>>(
                sortedWords.Select(kv => new Display.KeyValuePairViewModel<string, int> { Key = kv.Key, Value = kv.Value }));

            _dataGrid.ItemsSource = _keyValuePairs;
        }

    }
}
