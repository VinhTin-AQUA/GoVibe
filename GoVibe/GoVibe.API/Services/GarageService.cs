using Amazon.S3;
using Amazon.S3.Model;
using GoVibe.API.Constants;
using GoVibe.API.Models.Garages;

namespace GoVibe.API.Services
{
    public class GarageService
    {
        private readonly IAmazonS3 _s3Client;

        public GarageService(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }
        
        // UPLOAD
        public async Task<bool> Upload(IFormFile file)
        {
            var bucket = BucketNames.Images;

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            var request = new PutObjectRequest
            {
                BucketName = bucket,
                Key = file.FileName,
                InputStream = memoryStream,
                ContentType = file.ContentType,
                AutoCloseStream = false
            };
          
            request.UseChunkEncoding = false;
            var r = await _s3Client.PutObjectAsync(request);
            return r.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
        
        // DOWNLOAD
        public async Task<Stream> Download(string key)
        {
            var bucket = BucketNames.Images;
            var response = await _s3Client.GetObjectAsync(bucket, key);
            return response.ResponseStream;
        }
        
        public async Task<List<(string FileName, Stream Stream)>> DownloadManyAsync(List<string> keys)
        {
            var bucket = BucketNames.Images;
            var result = new List<(string, Stream)>();

            foreach (var key in keys)
            {
                var response = await _s3Client.GetObjectAsync(bucket, key);
                result.Add((key, response.ResponseStream));
            }
            return result;
        }
        
        // DELETE
        public async Task<bool> DeleteAsync(string key)
        {
            var bucket = BucketNames.Images;
            var r = await _s3Client.DeleteObjectAsync(bucket, key);
            return r.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        // GET ALL
        public async Task<List<S3ObjectModel>> GetAllAsync()
        {
            var result = new List<S3ObjectModel>();
            var request = new ListObjectsV2Request
            {
                BucketName = BucketNames.Images
            };

            ListObjectsV2Response response;
            do
            {
                response = await _s3Client.ListObjectsV2Async(request);
                result.AddRange(response.S3Objects.Select(x => new S3ObjectModel
                {
                    Key = x.Key,
                    Size = x.Size ?? 0,
                    LastModified = x.LastModified ?? null
                }));

                request.ContinuationToken = response.NextContinuationToken;
            } while (response.IsTruncated ?? false);

            return result;
        }
        
        public async Task<S3ObjectDetailDto> GetDetailAsync(string key)
        {
            var bucket = BucketNames.Images;
            var response = await _s3Client.GetObjectMetadataAsync(bucket, key);

            return new S3ObjectDetailDto
            {
                Key = key,
                ContentType = response.Headers.ContentType,
                Size = response.ContentLength,
                LastModified = response.LastModified ?? null
            };
        }

        public async Task DeleteManyAsync(List<string> keys)
        {
            var bucket = BucketNames.Images;
            if (!keys.Any())
                return;

            var request = new DeleteObjectsRequest
            {
                BucketName = bucket,
                Objects = keys.Select(k => new KeyVersion { Key = k }).ToList()
            };

            var response = await _s3Client.DeleteObjectsAsync(request);
            if (response.DeleteErrors != null && response.DeleteErrors.Count > 0)
            {
                var errors = response.DeleteErrors
                    .Select(e => $"{e.Key}: {e.Message}");

                throw new Exception("Delete failed: " + string.Join(", ", errors));
            }
        }
    }
}