using FileStorage.Layers.L00_BaseModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Globalization;

namespace FileStorage.Layers.L01_Entities
{
    public class FileEntity:BaseEntity
    {
        [Required]
        public string RelativePath { get; set; }
        [Required]
        public string RootPath { get; set; }
        public long ClientId { get; set; }
        [Required]
        public DateTime? CreationDate { get; set; }
        [Column(TypeName= "Date")]
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public int StartTime { get; set; }
        [Required]
        public Guid AccessToken { get; set; }

        [MaxLength(20)]
        [Required]
        public string FileType { get; set; }
        [Required]
        public long? FileSize { get; set; }

        [ForeignKey("ClientId")]
        public virtual ClientEntity client { get; set; }
    }
}