using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPF_App.Interfaces;

namespace WPF_App.Methods
{
    public class Display : IDisplay
    {
        private TextBox _rawTextBox;
        private TextBox _outputTextBox;

        public Display(TextBox rawTextBox, TextBox outputTextBox)
        {
            _rawTextBox = rawTextBox;
            _outputTextBox = outputTextBox;
        }

        public async Task DisplayRawText(string text)
        {
            _rawTextBox.Clear();
            _rawTextBox.Text = text;
        }

        public async Task DisplayOutputText(Dictionary<string, int> words)
        {
            _outputTextBox.Clear();
            _outputTextBox.Text = "Word\t\t\tCount\n";

            foreach (var word in words)
            {
                // Correctly formats words up to 24 characters long
                if (word.Key.Length < 8)
                {
                    _outputTextBox.Text += word.Key + "\t\t\t" + word.Value + "\n";
                }
                else if (word.Key.Length >= 8 && word.Key.Length < 16)
                {
                    _outputTextBox.Text += word.Key + "\t\t" + word.Value + "\n";
                }
                else
                {
                    _outputTextBox.Text += word.Key + "\t" + word.Value + "\n";
                }
            }
        }

    }
}
