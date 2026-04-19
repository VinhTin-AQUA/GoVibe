using Elastic.Clients.Elasticsearch;

namespace GoVibeSearch.API.Services
{
    public interface IElasticService<T> where T : class
    {
        Task<bool> IndexAsync(T document, string? index = null);
        Task<bool> IndexManyAsync(IEnumerable<T> documents, string? index = null);
        Task<T?> GetAsync(string id, string? index = null);
        Task<bool> DeleteAsync(string id, string? index = null);
        Task<List<T>> SearchAsync(Func<SearchRequestDescriptor<T>, SearchRequestDescriptor<T>> selector);
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<bool> DeleteAllRecordAsync();
        Task<bool> DeleteIndexAsync();
        Task<long> CountPlacesAsync();
    }

    public class ElasticService<T> : IElasticService<T> where T : class
    {
        protected readonly ElasticsearchClient _client;
        protected string _defaultIndex;

        public ElasticService(ElasticsearchClient client)
        {
            _client = client;
            _defaultIndex = "";
        }

        public async Task<bool> IndexAsync(T document, string? index = null)
        {
            var response = await _client.IndexAsync(document, i => i
                .Index(index ?? _defaultIndex)
            );

            return response.IsValidResponse;
        }

        public async Task<bool> IndexManyAsync(IEnumerable<T> documents, string? index = null)
        {
            var response = await _client.BulkAsync(b => b
                .Index(index ?? _defaultIndex)
                .IndexMany(documents)
            );

            return !response.Errors;
        }

        public async Task<T?> GetAsync(string id, string? index = null)
        {
            var response = await _client.GetAsync<T>(id, g => g
                .Index(index ?? _defaultIndex)
            );

            return response.Source;
        }

        public async Task<bool> DeleteAsync(string id, string? index = null)
        {
            var response = await _client.DeleteAsync<T>(id, d => d
                .Index(index ?? _defaultIndex)
            );

            return response.IsValidResponse;
        }

        public async Task<List<T>> SearchAsync(
            Func<SearchRequestDescriptor<T>, SearchRequestDescriptor<T>> selector)
        {
            var response = await _client.SearchAsync<T>(s =>
                selector(s.Indices(_defaultIndex))
            );

            return response.Documents.ToList();
        }
        
        public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var response = await _client.SearchAsync<T>(s => s
                    .Indices(_defaultIndex)
                    .Size(1000) // total record
                    .TrackTotalHits(false)
                    .Source(src => src
                            .Filter(i => i.Includes("*")) // or only field
                    )
                    .Query(q => q.MatchAll()),
                cancellationToken
            );

            if (!response.IsValidResponse)
            {
                var error = response.ElasticsearchServerError?.Error?.Reason;
                var debug = response.DebugInformation;

                throw new Exception($"Elastic failed: {error}\n{debug}");
            }

            return response.Documents.ToList();
        }

        public async Task<bool> DeleteAllRecordAsync()
        {
            var r = await _client.DeleteByQueryAsync<object>(d => d.Indices(_defaultIndex).Query(q => q.MatchAll()));
            return r.IsSuccess();
        }

        public async Task<bool> DeleteIndexAsync()
        {
           var r = await _client.Indices.CreateAsync(_defaultIndex);
           return r.IsSuccess();
        }
        
        public async Task<long> CountPlacesAsync()
        {
            var response = await _client.CountAsync<object>(c => c
                .Indices(_defaultIndex)
            );

            if (!response.IsValidResponse)
                throw new Exception(response.ElasticsearchServerError?.Error?.Reason);

            return response.Count;
        }
    }
}
