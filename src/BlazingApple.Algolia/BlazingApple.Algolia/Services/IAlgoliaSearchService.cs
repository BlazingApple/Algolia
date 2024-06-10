using Algolia.Search.Models.Search;
using BlazingApple.Algolia.Models;

namespace BlazingApple.Algolia.Services;

/// <summary>I/O for searching services.</summary>
public interface IAlgoliaSearchService
{
	/// <summary>Search the index for something.</summary>
	/// <param name="query"><see cref="Query"/></param>
	/// <param name="indexName">The name of the index to search, defaults to app settings if not specified.</param>
	/// <returns><see cref="SearchResponse{T}"/></returns>
	Task<SearchResponse<SearchResult<T>>> Search<T>(Query query, string? indexName = null)
		where T : class;
}