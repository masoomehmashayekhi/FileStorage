using FileStorage.Shared.Common.Exceptions;
using System.Collections.Generic;


namespace FileStorage.Layers.L00_BaseModels
{
    public class RequestResultViewModel
    {
        public object Data { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public List<FileStorageErrorMessage> DetailErrorList { get; set; }=new List<FileStorageErrorMessage>();
        public Dictionary<string, string> ValidationMessages { get; set; }

    }
}
