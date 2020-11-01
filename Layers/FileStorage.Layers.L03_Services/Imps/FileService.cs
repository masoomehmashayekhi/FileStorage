
using FileStorage.Layers.L00_BaseModels;
using FileStorage.Layers.L00_BaseModels.Errors;
using FileStorage.Layers.L01_Entities;
using FileStorage.Layers.L02_DataLayer;
using FileStorage.Layers.L03_Services.Contracts;
using FileStorage.Layers.L04_ViewModels;
using FileStorage.Shared.Common;
using FileStorage.Shared.Common.Exceptions;
using FileStorage.Shared.Common.FileUtility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xabe.FFmpeg;

namespace FileStorage.Layers.L03_Services
{
    public class FileService : IFileService
    {
        private readonly IFileStorageRepo _data;
        private readonly IOptionsSnapshot<Settings>  _settings;
        private IMemoryCache _cache;

        public FileService(IFileStorageRepo context, IOptionsSnapshot<Settings> setting, IMemoryCache cache)
        {
            _data = context;
            _settings = setting;
            _cache = cache;
        }



        public async Task<DownloadViewModel> DownloadAsync( long fileId,string accessToken,bool thumbnail)
        {
            try
            {
                FileEntity fileAddress = null;
                var cashfile = CacheTryGetValueSet(accessToken);
                if (cashfile != null)
                    fileAddress = cashfile;
                else
                {
                    fileAddress = await _data.GetFileEntity(fileId);
                    CacheValueSet(accessToken, fileAddress);
                }
                DownloadViewModel downloadViewModel = new DownloadViewModel(); 
                
                if (fileAddress != null)
                { 
                    string AbPath="";
                    if (!thumbnail)
                        AbPath = Path.Combine(fileAddress.RootPath, fileAddress.RelativePath, accessToken + "." + fileAddress.FileType);
                    else
                    {
                        AbPath=await CreateThumbnail(fileAddress);

                    }
                    if (File.Exists(AbPath))
                    {
                        if (fileAddress.AccessToken.ToString().Equals(accessToken))
                        {
                            var memory = new MemoryStream();
                            try
                            {
                                using (var stream = new FileStream(AbPath, FileMode.Open))
                                {
                                    await stream.CopyToAsync(memory);
                                }
                            }catch(Exception ex)
                            {
                                
                              
                            }
                            memory.Position = 0;
                            downloadViewModel.ContentType = ContentType.GetContentType(AbPath);
                            downloadViewModel.FileName = Path.GetFileName(AbPath);
                            downloadViewModel.Memory = memory;
                        }
                    }


                }
                return downloadViewModel;

            }
            catch (Exception ex)
            {
                throw new FileStorageException(ErrorMessages.FileDownloadError, ex);
            }

        }


        public async Task<UpLoadViewModel> UploadAsync(long client, IFormFile file)
        {
            try
            {
                if (string.IsNullOrEmpty(file.FileName) || file.Length == 0)
                {
                    throw new FileStorageException(ErrorMessages.UploadFileValidError);
                }
                var accesstoken = Guid.NewGuid();
                var extension = file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                var fileName = accesstoken.ToString() + "." + extension;
                DateTime now = DateTime.Now;
                var path = Path.Combine(client.ToString(),now.Year.ToString(),now.Month.ToString(),now.Day.ToString(),now.Hour.ToString());
                var abPath = Path.Combine(_settings.Value.PhysicalPath, path);
                Directory.CreateDirectory(abPath);
                using (var bits = new FileStream(Path.Combine(abPath,fileName), FileMode.Create))
                {
                    await file.CopyToAsync(bits);
                }
               
                var uploadFile = new FileEntity()
                {
                    RootPath = _settings.Value.PhysicalPath,
                    RelativePath= path,
                    ClientId = client,
                    CreationDate = now,
                    AccessToken = accesstoken,
                    StartTime = (int)now.TimeOfDay.TotalMilliseconds,
                    StartDate = now,
                    FileSize = file.Length,
                    FileType = extension
                };
                var Id=await _data.AddFileEntity(uploadFile);
                new Thread(() =>
                {
                    try
                    {
                        CreateThumbnail(uploadFile);
                    }
                    catch
                    {

                    }
                }).Start();
                CacheValueSet(accesstoken.ToString(), uploadFile);
                return new UpLoadViewModel { FileId = Id, AccessToken=accesstoken};

            }
            catch (Exception ex)
            {

                throw new FileStorageException(ErrorMessages.UploadFileError, ex);
            }
        }
        public async  Task<string> CreateThumbnail(FileEntity fileAddress)
        {
            string AbPath = "";
            try
            {
                var extension = fileAddress.FileType;
                string accessToken = fileAddress.AccessToken.ToString();
                AbPath = Path.Combine(fileAddress.RootPath, fileAddress.RelativePath, accessToken + "_thumb.jpg");
                if (extension == "mp4" || extension == "avi" || extension == "mp3")
                {
                    
                    if (!File.Exists(AbPath))
                    {
                        var tempPath = Path.Combine(fileAddress.RootPath, fileAddress.RelativePath, accessToken + "_temp.jpg");
                        if (!File.Exists(tempPath))
                            await GetThumbnailFromVideo(Path.Combine(fileAddress.RootPath, fileAddress.RelativePath, accessToken + "." + fileAddress.FileType), Path.Combine(fileAddress.RootPath, fileAddress.RelativePath, accessToken + "_temp.jpg"));

                        GetReducedImage(200, 200, new FileStream(Path.Combine(fileAddress.RootPath, fileAddress.RelativePath, accessToken + "_temp.jpg"), FileMode.Open), AbPath);

                    }
                }
                else if (extension == "jpg" || extension == "jpeg" || extension == "png" || extension == "gif" || extension == "bmp")
                {
                    if (!File.Exists(AbPath))
                    {
                        GetReducedImage(200, 200, new FileStream(Path.Combine(fileAddress.RootPath, fileAddress.RelativePath, accessToken + "." + fileAddress.FileType), FileMode.Open), AbPath);
                    }
                }
            }
            catch(Exception ex)
            {
                if (ex.Message.Contains("is being used by another process"))
                {
                    var processes = FileUtil.WhoIsLocking(AbPath);
                    if (processes.Count > 0) // deal with the file lock
                    {
                        foreach (Process p in processes)
                        {
                            if (p.MachineName == ".")
                                FileUtil.localProcessKill(p.ProcessName);
                        }
                    }
                }
            }
            return AbPath;
        }

        public async Task<bool>  GetThumbnailFromVideo(string video,string outputPath)
        {
            IConversion conversion =  FFmpeg.Conversions.FromSnippet.Snapshot(video, outputPath, TimeSpan.FromSeconds(1)).Result;
            IConversionResult result = await conversion.Start();
            
            return true;

        }
        public void GetReducedImage(int width, int height, Stream resourceImage,string outputPath)
        {
            try
            {
                using (Image image = Image.FromStream(resourceImage))
                {
                    Image thumb = image.GetThumbnailImage(width, height, () => false, IntPtr.Zero);
                    thumb.Save(outputPath);
                    thumb.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new FileStorageException(ErrorMessages.UploadFileError, ex);
            }
        }

        public FileEntity CacheTryGetValueSet(string accessToken)
        {
            FileEntity cacheEntry;

            // Look for cache key.
            if (_cache.TryGetValue(accessToken, out cacheEntry))
            {
                return cacheEntry;
            }
            else
                return null;

        }

        public bool CacheValueSet(string accessToken, FileEntity cacheEntry)
        {
            try
            {
                // Look for cache key.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                // Keep in cache for this time, reset time if accessed.
                .SetSlidingExpiration(TimeSpan.FromHours(3));

                // Save data in cache.
                _cache.Set(accessToken, cacheEntry, cacheEntryOptions);
                return true;
            }
            catch
            {
                return false;
            }

        }

    }
}
