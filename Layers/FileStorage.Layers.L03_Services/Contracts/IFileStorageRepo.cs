using FileStorage.Layers.L01_Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FileStorage.Layers.L03_Services.Contracts
{
    public interface IFileStorageRepo
    {
        Task<FileEntity> GetFileEntity(long id);
        Task<List<ClientEntity>> GetClientEntity();
        Task<long> AddFileEntity(FileEntity file);
    }
}
