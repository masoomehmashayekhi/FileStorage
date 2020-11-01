using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;

namespace FileStorage.Layers.L04_ViewModels
{
    public class DownloadViewModel
    {
        public string ContentType { get; set; }
        
        public string FileName { get; set; }
        public MemoryStream Memory { get; set; }
    }
}
