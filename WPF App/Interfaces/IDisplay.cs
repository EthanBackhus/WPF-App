﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_App.Interfaces
{
    public interface IDisplay
    {
        public Task DisplayRawText(string text);
        public Task DisplayOutputText(Dictionary<string, int> words);
    }
}
