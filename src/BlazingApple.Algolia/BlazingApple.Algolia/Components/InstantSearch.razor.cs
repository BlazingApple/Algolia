using Algolia.Search.Models.Search;
using BlazingApple.Algolia.Models;
using BlazingApple.Algolia.Services;
using Microsoft.AspNetCore.Components;

namespace BlazingApple.Algolia.Components;

/// <summary>Search component.</summary>
/// <typeparam name="T">The type of results</typeparam>
public partial class InstantSearch<T> : ComponentBase
	where T : class
{
	private bool _isLoading;
	private int _pageSize = 10;

	private SearchResponse<SearchResult<T>>? _response;
	private int _page;
	/// <summary>The user's query.</summary>
	private string? SearchQuery { get; set; }

	/// <summary>Apply settings to the query.</summary>
	[Parameter]
	public EventCallback<Query> ApplyQuerySettings { get; set; }

	/// <summary>Rendering logic for a result.</summary>
	[Parameter]
	public RenderFragment<T>? ResultTemplate { get; set; }

	[Inject]
	private IAlgoliaSearchService SearchService { get; set; } = null!;

	private async Task Search()
	{
		_isLoading = true;
		Query query = new(SearchQuery)
		{
			HitsPerPage = _pageSize,
			Page = _page,
		};

		if (ApplyQuerySettings.HasDelegate)
		{
			await ApplyQuerySettings.InvokeAsync(query);
		}

		_response = await SearchService.Search<T>(query);
		_page = _response.Page;
		_isLoading = false;
	}
}