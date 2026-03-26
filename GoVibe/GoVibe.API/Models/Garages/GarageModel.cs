namespace GoVibe.API.Models.Garages
{
    public class GarageModel
    {
        
    }
    
    public class S3ObjectModel
    {
        public string Key { get; set; } = "";
        public long Size { get; set; }
        public DateTime? LastModified { get; set; } = null;
    }
    
    public class S3ObjectDetailDto
    {
        public string Key { get; set; } = "";
        public string ContentType { get; set; } = "";
        public long Size { get; set; }
        public DateTime? LastModified { get; set; }
    }
}