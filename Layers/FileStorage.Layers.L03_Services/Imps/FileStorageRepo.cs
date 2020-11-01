using FileStorage.Layers.L00_BaseModels;
using FileStorage.Layers.L00_BaseModels.Errors;
using FileStorage.Layers.L01_Entities;
using FileStorage.Layers.L02_DataLayer;
using FileStorage.Layers.L03_Services.Contracts;
using FileStorage.Shared.Common;
using FileStorage.Shared.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace FileStorage.Layers.L03_Services.Imps
{
    public class FileStorageRepo : IFileStorageRepo,IDisposable
    {
        private readonly FileStorageDbContext _context;

        public FileStorageRepo(FileStorageDbContext context)
        {
            _context = context;
            _context.CheckArgumentIsNull(nameof(_context));
        }
        public async Task<long> AddFileEntity(FileEntity file)
        {
            try
            {
                var Entity=await _context.Files.AddAsync(file);
                await _context.SaveChangesAsync();
                return Entity.Entity.Id;
            }
            catch (Exception ex)
            {
                throw new FileStorageException(ErrorMessages.AddtoDb, ex);
            }
        }

        public void Dispose()
        {
            
        }

        public async Task<List<ClientEntity>> GetClientEntity()
        {
            return await _context.Clients.ToListAsync();
        }

        public async Task<FileEntity> GetFileEntity(long id)
        {
            try
            {
                var fileAddress = await _context.Files.FindAsync(id);
                return fileAddress;
            }
            catch (Exception ex)
            {
                throw new FileStorageException(ErrorMessages.GetFromDb, ex);
            }
        }

    }

}
