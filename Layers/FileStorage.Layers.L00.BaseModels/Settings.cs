using System;
using System.Collections.Generic;
using System.Text;

namespace FileStorage.Layers.L00_BaseModels
{
    public class Settings
    {
        public string PhysicalPath { get; set; }
        public string[] ValidFileUpload { get; set; }
        public long MaxFileSize { get; set; }
    }
}
