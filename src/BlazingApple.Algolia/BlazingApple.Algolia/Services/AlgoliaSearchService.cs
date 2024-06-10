using Algolia.Search.Clients;
using Algolia.Search.Models.Search;
using BlazingApple.Algolia.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BlazingApple.Algolia.Services;

/// <summary>Client search service.</summary>
public class AlgoliaSearchService : IAlgoliaSearchService
{
	private readonly ISearchClient _client;
	private readonly ILogger<AlgoliaSearchService> _logger;
	private readonly string _applicationId;
	private readonly string _apiKey;
	private readonly string _index;

	/// <summary>DI Constructor</summary>
	public AlgoliaSearchService(HttpClient http, ILogger<AlgoliaSearchService> logger, IConfiguration configuration)
	{
		IConfigurationSection algoliaSection = configuration.GetRequiredSection("Algolia");
		_apiKey = algoliaSection["ApiKey"] ?? "c79b2e61519372a99fa5890db070064c"; // Api key;
		_applicationId = algoliaSection["ApplicationId"] ?? "M19DXW5X0Q"; // Font Awesome id;
		_index = algoliaSection["Index"] ?? "fontawesome_com-splayed-5.15.4";
		_client = new SearchClient(_applicationId, _apiKey);
		_logger = logger;

	}

	/// <inheritdoc />
	public async Task<SearchResponse<SearchResult<T>>> Search<T>(Query query, string? indexName = null)
		where T : class
	{
		indexName ??= _index;
		SearchIndex index = _client.InitIndex(indexName);

		SearchResponse<object>? result = await index.SearchAsync<object>(query);
		string jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);

		SearchResponse<SearchResult<T>> returnVal = CloneResults<T>(jsonResult);

		return returnVal ?? throw new InvalidDataException();
	}

	private SearchResponse<SearchResult<T>> CloneResults<T>(string jsonResult)
		where T : class
	{
		JsonSerializerOptions options = new();
		SearchResponse<SearchResult<T>>? searchResult = JsonSerializer.Deserialize<SearchResponse<SearchResult<T>>>(jsonResult, options);
		SearchResponse<T>? actualResult = JsonSerializer.Deserialize<SearchResponse<T>>(jsonResult, options);
		SearchResponse<ResultMetadata>? metadataResult = JsonSerializer.Deserialize<SearchResponse<ResultMetadata>>(jsonResult, options);

		for (int i = 0; i < searchResult!.Hits.Count; i++)
		{
			searchResult.Hits[i] = new SearchResult<T>()
			{
				Result = actualResult!.Hits[i],
				Metadata = metadataResult!.Hits[i]
			};
		}

		return searchResult ?? throw new ArgumentOutOfRangeException();
	}
}