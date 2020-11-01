using FileStorage.Layers.L01_Entities;
using FileStorage.Layers.L04_ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FileStorage.Layers.L03_Services.Contracts
{
    public interface IFileService
    {
        Task<UpLoadViewModel> UploadAsync( long client, IFormFile file);
        Task<DownloadViewModel> DownloadAsync(long fileId,string accessToken, bool thumbnail);
        Task<bool> GetThumbnailFromVideo(string video, string outputPath);
        void GetReducedImage(int width, int height, Stream resourceImage, string outputPath);
    }
}
