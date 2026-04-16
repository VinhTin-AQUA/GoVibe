using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using GoVibeSearch.API.Models;

namespace GoVibeSearch.API.Services
{
    public interface IPlaceSearchService : IElasticService<PlaceSearchModel>
    {
        Task<List<PlaceSearchModel>> SearchByNameAsync(string keyword);
        Task<List<PlaceSearchModel>> SearchByAddressAsync(string address);
        Task<List<PlaceSearchModel>> SearchByCountryAsync(string country);

        Task<List<PlaceSearchModel>> SearchByCategoryAsync(Guid categoryId);
        Task<List<PlaceSearchModel>> SearchByCategoryNameAsync(string categoryName);

        Task<List<PlaceSearchModel>> SearchTopRatedAsync(int size = 10);
        Task<List<PlaceSearchModel>> SearchMostViewedAsync(int size = 10);

        Task<List<PlaceSearchModel>> SearchAdvancedAsync(string? keyword, string? country, Guid? categoryId, double? minRating);
    }

    public class PlaceSearchService : ElasticService<PlaceSearchModel>, IPlaceSearchService
    {
        public PlaceSearchService(ElasticsearchClient client) : base(client)
        {
        }

        public async Task<List<PlaceSearchModel>> SearchByNameAsync(string keyword)
        {
            return await SearchAsync(s => s
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Name)
                        .Query(keyword)
                    )
                )
            );
        }

        public async Task<List<PlaceSearchModel>> SearchByAddressAsync(string address)
        {
            return await SearchAsync(s => s
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Address)
                        .Query(address)
                    )
                )
            );
        }

        public async Task<List<PlaceSearchModel>> SearchByCountryAsync(string country)
        {
            return await SearchAsync(s => s
                .Query(q => q
                    .Term(t => t
                        .Field(f => f.Country)
                        .Value(country)
                    )
                )
            );
        }

        // 🔥 Nested Categories (QUAN TRỌNG)
        public async Task<List<PlaceSearchModel>> SearchByCategoryAsync(Guid categoryId)
        {
            return await SearchAsync(s => s
                .Query(q => q
                    .Nested(n => n
                        .Path(p => p.Categories)
                        .Query(nq => nq
                            .Term(t => t
                                .Field("categories.id")
                                .Value(categoryId.ToString())
                            )
                        )
                    )
                )
            );
        }

        public async Task<List<PlaceSearchModel>> SearchByCategoryNameAsync(string categoryName)
        {
            return await SearchAsync(s => s
                .Query(q => q
                    .Nested(n => n
                        .Path(p => p.Categories)
                        .Query(nq => nq
                            .Match(m => m
                                .Field("categories.name")
                                .Query(categoryName)
                            )
                        )
                    )
                )
            );
        }

        public async Task<List<PlaceSearchModel>> SearchTopRatedAsync(int size = 10)
        {
            return await SearchAsync(s => s
                .Size(size)
                .Sort(sort => sort
                    .Field(f => f
                        .Field(p => p.TotalRating)
                        .Order(SortOrder.Desc)
                    )
                )
            );
        }

        public async Task<List<PlaceSearchModel>> SearchMostViewedAsync(int size = 10)
        {
            return await SearchAsync(s => s
                .Size(size)
                .Sort(so => so
                        .Field(f => f.Field(p => p.TotalViews)
                        .Order(SortOrder.Desc)
                    )
                )
            );
        }

        public async Task<List<PlaceSearchModel>> SearchAdvancedAsync(string? keyword, string? country, Guid? categoryId, double? minRating)
        {
            return await SearchAsync(s => s
                .Query(q => q
                    .Bool(b => b
                        .Must(BuildMust())
                        .Filter(BuildFilter())
                    )
                )
            );

            // ================= MUST =================
            List<Query> BuildMust()
            {
                var list = new List<Query>();

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    list.Add(new MatchQuery
                    {
                        Field = "name",
                        Query = keyword
                    });
                }

                if (!string.IsNullOrWhiteSpace(country))
                {
                    list.Add(new TermQuery
                    {
                        Field = "country",
                        Value = country
                    });
                }
                //Elastic.Clients.Elasticsearch.QueryDsl.NumberRangeQuery
                if (minRating.HasValue)
                {
                    list.Add(new NumberRangeQuery
                    {
                        Field = "totalRating",
                        Gte = minRating.Value
                    });
                }

                return list;
            }

            // ================= FILTER =================
            List<Query> BuildFilter()
            {
                if (!categoryId.HasValue)  return [];

                return new List<Query>
                {
                    new NestedQuery
                    {
                        Path = "categories",
                        Query = new TermQuery
                        {
                            Field = "categories.id",
                            Value = categoryId.Value.ToString()
                        }
                    }
                };
            }
        }
    }
}
