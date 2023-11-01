﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WPF_App.Interfaces
{
    public interface IFileProcessor
    {
        Task ProcessFileAsync(string filePath, IProgress<int> progress, CancellationToken cancellationToken);
    }
}
