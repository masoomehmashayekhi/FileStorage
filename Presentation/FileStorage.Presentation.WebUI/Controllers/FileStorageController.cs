using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FileStorage.Layers.L00_BaseModels;
using FileStorage.Layers.L03_Services.Contracts;
using FileStorage.Layers.L04_ViewModels;
using FileStorage.Presentation.WebUI.Cache;
using FileStorage.Presentation.WebUI.TokenAuth;
using FileStorage.Shared.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Xabe.FFmpeg;

namespace FileStorage.Presentation.WebUI.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    [ApiController]
    public class FileStorageController : BaseController
    {
        private IFileService _fileService;
        private readonly IHttpContextAccessor _accessor;
        private readonly IOptionsSnapshot<Settings> _setting;

        public FileStorageController(IFileService fileService,IHttpContextAccessor accessor, IOptionsSnapshot<Settings> setting, IMemoryCache cache)
        {
            _fileService = fileService;
            _fileService.CheckArgumentIsNull(nameof(_fileService));
            _accessor = accessor;
            _setting = setting;
        }

        [HttpGet("[action]"), HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            List<Claim> cp=_accessor.HttpContext.User.Claims.ToList();
            if (file.Length > _setting.Value.MaxFileSize)
                return BadRequest(GetRequestResult(new Exception("File is too big!")));
            var extension = file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            if (!_setting.Value.ValidFileUpload.Any(x=>x==extension))
                return BadRequest(GetRequestResult(new Exception("File format is not correct!")));
          
            return Ok(GetRequestResult(await _fileService.UploadAsync(long.Parse(cp[1].Value), file)));
        }


        [HttpGet("[action]/{fileId}/{accessToken}"), HttpPost("[action]/{fileId}/{accessToken}")]
        [AllowAnonymous]

        public async Task<IActionResult> Download(long fileId,string accessToken)
        {
            try
            {
                var response = await _fileService.DownloadAsync(fileId, accessToken,false);
                if (response.Memory is null)
                    return NotFound();
                return File(response.Memory, response.ContentType, response.FileName, true);
            }
            catch
            {
                return BadRequest();
            }


        }
        [HttpGet("[action]/{fileId}/{accessToken}"), HttpPost("[action]/{fileId}/{accessToken}")]
        [AllowAnonymous]

        public async Task<IActionResult> Download_thumb(long fileId, string accessToken)
        {
            try
            {
                var response = await _fileService.DownloadAsync(fileId, accessToken,true);
                if (response.Memory is null)
                    return NotFound();
                return File(response.Memory, response.ContentType, response.FileName, true);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }


        }


       
    }
}
