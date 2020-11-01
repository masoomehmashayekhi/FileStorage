using System;
using System.Collections.Generic;
using System.Text;

namespace FileStorage.Shared.Common.Exceptions
{
   public  class FileStorageException:Exception
    {
        public FileStorageException(FileStorageErrorMessage error)
        {
            UserMessage = error.Message;
            ErrorCode = error.Code;
            ExceptionList = null;
        }

        public FileStorageException(FileStorageErrorMessage error, Exception e)
        {
            UserMessage = error.Message;
            ErrorCode = error.Code;
            ExceptionList = new List<Exception> { e };
        }
        public FileStorageException(FileStorageErrorMessage error, params Exception[] exceptions)
        {
            UserMessage = error.Message;
            ErrorCode = error.Code;
            ExceptionList = new List<Exception>();
            ExceptionList.AddRange(exceptions);
        }



        public string UserMessage { get; set; }
        public string ErrorCode { get; set; }
        public List<Exception> ExceptionList { get; set; }
    }
}
