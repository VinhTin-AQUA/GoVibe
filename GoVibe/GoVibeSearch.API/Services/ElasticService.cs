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
        Task<List<T>> GetAllAsync();
    }

    public class ElasticService<T> : IElasticService<T> where T : class
    {
        private readonly ElasticsearchClient _client;
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
        
        public async Task<List<T>> GetAllAsync()
        {
            var response = await _client.SearchAsync<T>(s => s
                .Indices(_defaultIndex)
                .Size(10000) // max default
                .Query(q => q.MatchAll())
            );

            if (!response.IsValidResponse)
                throw new Exception("Elastic search failed");

            if (!response.IsValidResponse)
            {
                var error = response.ElasticsearchServerError?.Error?.Reason;
                var debug = response.DebugInformation;

                throw new Exception($"Elastic failed: {error}\n{debug}");
            }

            return response.Documents.ToList();
        }
    }
}
