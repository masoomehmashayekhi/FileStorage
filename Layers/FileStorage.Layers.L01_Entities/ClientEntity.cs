using FileStorage.Layers.L00_BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace FileStorage.Layers.L01_Entities
{
    public class ClientEntity:BaseEntity
    {
        [DataType(DataType.Text), MaxLength(100)]
        public string Name { get; set; }

        public string IPAddress { get; set; }

        public Guid Token { get; set; }

        public virtual ICollection<FileEntity> Files { get; set; }
    }
}
