using FileStorage.Shared.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileStorage.Layers.L00_BaseModels.Errors
{
    public class ErrorMessages
    {
        public static readonly FileStorageErrorMessage UploadFileError =
        new FileStorageErrorMessage("UFE-1000", "هنگام آپلود فایل خطایی رخ داده است.");
        public static readonly FileStorageErrorMessage UploadFileValidError =
          new FileStorageErrorMessage("UFE-1001", "فایل ارسالی بایستی معتبر باشد.");

        public static FileStorageErrorMessage FileNotFoundError =
              new FileStorageErrorMessage("UFE-1002", "فایل وجود ندارد.");
        public static FileStorageErrorMessage FileIdNotFoundError =
             new FileStorageErrorMessage("UFE-1003", "شناسه فایل وجود ندارد.");
        public static FileStorageErrorMessage FileDownloadError =
             new FileStorageErrorMessage("UFE-1004", "خطا در دانلود فایل.");
        public static FileStorageErrorMessage GetFromDb =
             new FileStorageErrorMessage("UFE-1005", "خطا در دریافت اطلاعات.");
        public static FileStorageErrorMessage AddtoDb =
            new FileStorageErrorMessage("UFE-1006", "خطا در ثبت اطلاعات.");
    }
}
