namespace GoVibe.API.Configurations
{
    public class GarageConfig
    {
        public string ServiceURL { get; set; } = "";
        public string AccessKey { get; set; } = "";
        public string SecretKey { get; set; } = "";
        public string BucketName { get; set; } = "";
    }
    
    public class BucketPrefixKeyNames
    {
        public const string PlaceImages = "place-images";
        public const string PlaceThumbnail = "place-thumbnail";
        public const string ReviewImages = "review-images";
        
    }
}
