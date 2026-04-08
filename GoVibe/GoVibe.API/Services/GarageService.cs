using Amazon.S3;
using Amazon.S3.Model;
using GoVibe.API.Configurations;
using GoVibe.API.Models.Garages;
using Microsoft.Extensions.Options;

namespace GoVibe.API.Services
{
    public class GarageService
    {
        //private readonly IAmazonS3 _s3Client;
        //private readonly IOptions<GarageConfig> _garageConfig;
        //private readonly string _bucketName;

        //public GarageService(IAmazonS3 s3Client, IOptions<GarageConfig> garageConfig)
        public GarageService()
        {
            //_s3Client = s3Client;
            //_garageConfig = garageConfig;
            //_bucketName = _garageConfig.Value.BucketName;
        }
        
        // UPLOAD
        public async Task<string> Upload(string prefix, IFormFile file)
        {
            //using var memoryStream = new MemoryStream();
            //await file.CopyToAsync(memoryStream);
            //memoryStream.Position = 0;
            string key = $"{prefix}/{file.FileName}";

            //var request = new PutObjectRequest
            //{
            //    BucketName = _bucketName,
            //    Key = key,
            //    InputStream = memoryStream,
            //    ContentType = file.ContentType,
            //    AutoCloseStream = false
            //};

            //request.UseChunkEncoding = false;
            //var r = await _s3Client.PutObjectAsync(request);
            //string result =  r.HttpStatusCode == System.Net.HttpStatusCode.OK ? key : "";
            //return result;

            return key;
        }
        
        // DOWNLOAD
        //public async Task<Stream> Download(string key)
        //{
        //    var bucket = _bucketName;
        //    var response = await _s3Client.GetObjectAsync(bucket, key);
        //    return response.ResponseStream;
        //}
        
        //public async Task<List<(string FileName, Stream Stream)>> DownloadManyAsync(List<string> keys)
        //{
        //    var bucket = _bucketName;
        //    var result = new List<(string, Stream)>();

        //    foreach (var key in keys)
        //    {
        //        var response = await _s3Client.GetObjectAsync(bucket, key);
        //        result.Add((key, response.ResponseStream));
        //    }
        //    return result;
        //}
        
        // DELETE
        public async Task<bool> DeleteAsync(string key)
        {
            //var bucket = _bucketName;
            //var r = await _s3Client.DeleteObjectAsync(bucket, key);
            //return r.HttpStatusCode == System.Net.HttpStatusCode.OK;

            return true;
        }

        // GET ALL
        //public async Task<List<S3ObjectModel>> GetAllAsync()
        //{
        //    var result = new List<S3ObjectModel>();
        //    var request = new ListObjectsV2Request
        //    {
        //        BucketName = _bucketName
        //    };

        //    ListObjectsV2Response response;
        //    do
        //    {
        //        response = await _s3Client.ListObjectsV2Async(request);
        //        result.AddRange(response.S3Objects.Select(x => new S3ObjectModel
        //        {
        //            Key = x.Key,
        //            Size = x.Size ?? 0,
        //            LastModified = x.LastModified ?? null
        //        }));

        //        request.ContinuationToken = response.NextContinuationToken;
        //    } while (response.IsTruncated ?? false);

        //    return result;
        //}
        
        //public async Task<S3ObjectDetailDto> GetDetailAsync(string key)
        //{
        //    var bucket = _bucketName;
        //    var response = await _s3Client.GetObjectMetadataAsync(bucket, key);

        //    return new S3ObjectDetailDto
        //    {
        //        Key = key,
        //        ContentType = response.Headers.ContentType,
        //        Size = response.ContentLength,
        //        LastModified = response.LastModified ?? null
        //    };
        //}

        public async Task DeleteManyAsync(List<string> keys)
        {
            //var bucket = _bucketName;
            //if (!keys.Any())
            //    return;

            //var request = new DeleteObjectsRequest
            //{
            //    BucketName = bucket,
            //    Objects = keys.Select(k => new KeyVersion { Key = k }).ToList()
            //};

            //var response = await _s3Client.DeleteObjectsAsync(request);
            //if (response.DeleteErrors != null && response.DeleteErrors.Count > 0)
            //{
            //    var errors = response.DeleteErrors
            //        .Select(e => $"{e.Key}: {e.Message}");

            //    throw new Exception("Delete failed: " + string.Join(", ", errors));
            //}
        }
    }
}