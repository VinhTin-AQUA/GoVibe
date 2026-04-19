using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using GoVibeSearch.API.Configs;
using GoVibeSearch.API.Models;
using Microsoft.Extensions.Options;

namespace GoVibeSearch.API.Services
{
    public interface IPlaceSearchService : IElasticService<PlaceSearchModel>
    {
        string GetIndexName();
        Task<List<PlaceSearchModel>> SearchByNameAsync(string keyword);
        Task<List<PlaceSearchModel>> SearchByAddressAsync(string address);
        Task<List<PlaceSearchModel>> SearchByCountryAsync(string country);
        Task<List<PlaceSearchModel>> SearchByCategoryAsync(Guid categoryId);
        Task<List<PlaceSearchModel>> SearchByCategoryNameAsync(string categoryName);
        Task<List<PlaceSearchModel>> SearchTopRatedAsync(int size = 10);
        Task<List<PlaceSearchModel>> SearchMostViewedAsync(int size = 10);
        Task<List<PlaceSearchModel>> SearchPlacesAsync(PlaceSearchRequest filter);
    }

    public class PlaceSearchService : ElasticService<PlaceSearchModel>, IPlaceSearchService
    {
        public PlaceSearchService(ElasticsearchClient client, IOptions<ElasticIndexes> options) : base(client)
        {
            _defaultIndex = options.Value.PlaceSearchIndex;
        }
        
        public string GetIndexName() => _defaultIndex;

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

        public async Task<List<PlaceSearchModel>> SearchPlacesAsync(PlaceSearchRequest filter)
        {
            var r =  await SearchAsync(s => s
                .From((filter.PageIndex - 1) * filter.PageSize)
                .Size(filter.PageSize)
                .Sort(ApplySort())
                .Query(q => q
                    .Bool(b => b
                        .Must(BuildMust())
                        .Filter(BuildFilter())
                        .Should(BuildShould())
                        .MinimumShouldMatch(0) // không bắt buộc should, chỉ boost ranking
                    )
                )
            );

            return r;

            // ================= MUST (SEARCH TEXT) =================
            List<Query> BuildMust()
            {
                var list = new List<Query>();

                if (!string.IsNullOrWhiteSpace(filter.Keyword))
                {
                    list.Add(new MultiMatchQuery
                    {
                        Fields = new[] { "name", "address" },
                        Query = filter.Keyword
                    });
                }

                return list;
            }

            // ================= FILTER (HARD CONDITIONS - AND) =================
            List<Query> BuildFilter()
            {
                var list = new List<Query>();

                if (!string.IsNullOrWhiteSpace(filter.Country))
                {
                    list.Add(new TermQuery
                    {
                        Field = "country",
                        Value = filter.Country
                    });
                }

                if (!string.IsNullOrWhiteSpace(filter.Status))
                {
                    list.Add(new TermQuery
                    {
                        Field = "status",
                        Value = filter.Status
                    });
                }

                if (filter.MinRating.HasValue)
                {
                    list.Add(new NumberRangeQuery
                    {
                        Field = "totalRating",
                        Gte = filter.MinRating.Value
                    });
                }

                if (filter.MinViews.HasValue)
                {
                    list.Add(new NumberRangeQuery
                    {
                        Field = "totalViews",
                        Gte = filter.MinViews.Value
                    });
                }

                return list;
            }

            // ================= SHOULD (BOOST RELEVANCE - OPTIONAL) =================
            List<Query> BuildShould()
            {
                var list = new List<Query>();

                if (filter.Tags != null && filter.Tags.Count > 0)
                {
                    list.Add(new TermsQuery
                    {
                        Field = "tags.keyword",
                        Terms = filter.Tags.Select(x => (FieldValue)x).ToArray()
                    });
                }

                if (filter.CategoryIds != null && filter.CategoryIds.Count > 0)
                {
                    list.Add(new TermsQuery
                    {
                        Field = "categories.id.keyword", // ⚠️ QUAN TRỌNG
                        Terms = filter.CategoryIds
                            .Select(x => (FieldValue)x.ToString())
                            .ToArray()
                    });
                }

                return list;
            }
            
            // ================= ApplySort =================
            List<SortOptions> ApplySort()
            {
                var sort = new List<SortOptions>();

                if (!string.IsNullOrWhiteSpace(filter.SortBy))
                {
                    // default sort: newest
                    sort.Add(new SortOptions
                    {
                        Field = new FieldSort
                        {
                            Field = "createdAt",
                            Order = SortOrder.Desc
                        }
                    });

                    if (filter.SortDesc.HasValue)
                    {
                        var order = filter.SortDesc.Value ? SortOrder.Desc : SortOrder.Asc;
                    
                        switch (filter.SortBy.ToLower())
                        {
                            case "rating":
                                sort.Add(new SortOptions
                                {
                                    Field = new FieldSort
                                    {
                                        Field = "totalRating",
                                        Order = order
                                    }
                                });
                                break;

                            case "views":
                                sort.Add(new SortOptions
                                {
                                    Field = new FieldSort
                                    {
                                        Field = "totalViews",
                                        Order = order
                                    }
                                });
                                break;

                            case "newest":
                            default:
                                sort.Add(new SortOptions
                                {
                                    Field = new FieldSort
                                    {
                                        Field = "createdAt",
                                        Order = SortOrder.Desc
                                    }
                                });
                                break;
                        }
                    }
                }
   
                return sort;
            }
        }
    }
}
