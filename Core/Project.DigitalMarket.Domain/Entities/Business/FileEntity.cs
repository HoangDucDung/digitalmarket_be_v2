using Project.DigitalMarket.Domain.Entities.Base;

namespace Project.DigitalMarket.Domain.Entities.Business
{
    public class FileEntity : BaseEntity
    {
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public string Base64Data { get; set; } = string.Empty; 
        public long FileSize { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
