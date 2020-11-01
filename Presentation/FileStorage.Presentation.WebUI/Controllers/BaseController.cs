using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileStorage.Layers.L00_BaseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileStorage.Presentation.WebUI.Controllers
{
    public class BaseController : Controller
    {
        protected RequestResultViewModel GetRequestResult(object data)
        {
            RequestResultViewModel res = new RequestResultViewModel
            {
                Data = data
            };
            return res;
        }
    }
}