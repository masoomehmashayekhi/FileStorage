using System;
using System.Collections.Generic;
using System.Linq;
using DNTPersianUtils.Core;
using FileStorage.Layers.L00_BaseModels.Errors;
using FileStorage.Layers.L01_Entities;
using FileStorage.Shared.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace FileStorage.Presentation.WebUI.Controllers
{
    public static class ControllerUtils
    {
       
        public static string GetClient(this HttpContext context)
        {
            try
            {

                return context.Connection.RemoteIpAddress.ToString();
            }

            catch (Exception e)
            {
                throw new FileStorageException(ControllerUtilsErrors.GetClientError, e);
            }
        }

       
      


    }
}