using FileStorage.Shared.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileStorage.Layers.L00_BaseModels.Errors
{
    public class ControllerUtilsErrors
    {
        public static readonly FileStorageErrorMessage GetClientError =
          new FileStorageErrorMessage("GUE-1000", "در بدست آوردن کاربر خطایی رخ داده است");
    }
}
