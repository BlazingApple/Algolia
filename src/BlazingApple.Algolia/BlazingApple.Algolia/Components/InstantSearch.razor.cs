using Algolia.Search.Models.Search;
using BlazingApple.Algolia.Models;
using BlazingApple.Algolia.Services;
using Flurl;
using Microsoft.AspNetCore.Components;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

namespace BlazingApple.Algolia.Components;

/// <summary>Search component.</summary>
/// <typeparam name="T">The type of results</typeparam>
public partial class InstantSearch<T> : ComponentBase
	where T : class
{
	private bool _isLoading;
	private int _pageSize = 10;
	private readonly string? _lastUrl;
	private int _page;
	private SearchResponse<SearchResult<T>>? _response;
	private bool _hasSearched;
	private readonly Dictionary<string, HashSet<string>> _selectedFacets = [];
	private IReadOnlyDictionary<string, FacetDetails>? _displayFacets => FacetsToDisplay?.ToDictionary(f => f.Key);


	/// <summary>The user's query.</summary>
	private string? SearchQuery { get; set; }

	[Inject]
	private NavigationManager NavManager { get; set; } = null!;

	/// <summary>Apply settings to the query.</summary>
	[Parameter]
	public EventCallback<Query> ApplyQuerySettings { get; set; }

	/// <summary>Rendering logic for a result.</summary>
	[Parameter]
	public RenderFragment<T>? ResultTemplate { get; set; }

	/// <summary>Facets that should be rendered to the user.</summary>
	[Parameter]
	public List<FacetDetails>? FacetsToDisplay { get; set; }

	/// <summary>Whether to search immediately on rendering the component.</summary>
	[Parameter]
	public bool SearchOnLoad { get; set; } = true;

	[Inject]
	private IAlgoliaSearchService SearchService { get; set; } = null!;

	/// <inheritdoc/>
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);

		if (firstRender && _response is null && SearchOnLoad)
		{
			_hasSearched = true;
			await PerformSearch(true, false);
		}
	}

	/// <inheritdoc />
	protected override async Task OnParametersSetAsync()
	{
		await base.OnParametersSetAsync();
		if (FacetsToDisplay is null)
		{
			return;
		}

		foreach (FacetDetails facetToDisplay in FacetsToDisplay)
		{
			if (!_selectedFacets.ContainsKey(facetToDisplay.Key))
			{
				_selectedFacets.Add(facetToDisplay.Key, []);
			}
		}
	}

	private async Task Search()
	{
		await PerformSearch(false, true);
	}

	private async Task<Query> GetQuery()
	{
		Query query = new(SearchQuery)
		{
			HitsPerPage = _pageSize,
			Page = _page,
			FacetFilters = ToFacetFilterString(_selectedFacets).ToList()
		};

		if (ApplyQuerySettings.HasDelegate)
		{
			await ApplyQuerySettings.InvokeAsync(query);
		}

		return query;
	}

	private async Task PerformSearch(bool isPageLoad, bool updateUrl)
	{
		_isLoading = true;
		NameValueCollection queryParams = HttpUtility.ParseQueryString(new Uri(NavManager.Uri).Query);

		JsonSerializerOptions options = new(JsonSerializerDefaults.Web);
		string? encodedQuery = queryParams["query"]!;
		string? serializedQuery = DecodeQueryPayload(encodedQuery);

		Query query;

		if (serializedQuery is not null or "{}" && isPageLoad)
		{
			query = JsonSerializer.Deserialize<Query>(serializedQuery!, options) ?? await GetQuery();
			SearchQuery = query.SearchQuery;
			_page = query.Page ?? 0;
			_pageSize = query.HitsPerPage ?? 10;
			UpdateSelectedFacetsFromQuery(query);
		}
		else
		{
			query = await GetQuery();
		}

		_response = await SearchService.Search<T>(query);
		if (updateUrl)
		{
			UpdateUrl(query);
		}

		_page = _response.Page;
		_isLoading = false;
		StateHasChanged();
	}

	private void UpdateSelectedFacetsFromQuery(Query query)
	{
		foreach (IEnumerable<string>? facetFilter in query.FacetFilters)
		{
			foreach (string? selectedFacet in facetFilter)
			{
				string[] keyAndFacet = selectedFacet.Split(':');
				if (!_selectedFacets.ContainsKey(keyAndFacet[0]))
				{
					_selectedFacets.Add(keyAndFacet[0], []);
				}

				_selectedFacets[keyAndFacet[0]].Add(keyAndFacet[1]);
			}
		}
	}

	private void UpdateUrl(Query query)
	{
		JsonSerializerOptions options = new(JsonSerializerDefaults.Web)
		{
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
		};

		string queryPayload = JsonSerializer.Serialize(query, options);
		Url newUri = NavManager.Uri.SetQueryParam("query", EncodeQueryPayload(queryPayload));

		NavManager.NavigateTo(newUri, false, true);
	}

	/// <summary>
	/// Combines selected facets using AND logic across categories and OR logic within them. 
	/// 
	/// See <![CDATA[https://www.algolia.com/doc/guides/managing-results/refine-results/filtering/in-depth/filters-and-facetfilters/#differences-between-filtering-parameters]]>
	/// </summary>
	/// <param name="allSelectedFacets"></param>
	/// <returns>Faceting string</returns>
	private IEnumerable<List<string>> ToFacetFilterString(Dictionary<string, HashSet<string>> allSelectedFacets)
	{
		foreach (KeyValuePair<string, HashSet<string>> facetsInCategory in allSelectedFacets)
		{
			if (facetsInCategory.Value.Count > 0)
			{
				yield return ToFacetFilterString(facetsInCategory.Key, facetsInCategory.Value).ToList();
			}
		}
	}

	private IEnumerable<string> ToFacetFilterString(string key, HashSet<string> selectedFacets)
	{
		foreach (string selectedFacet in selectedFacets)
		{
			yield return $"{key}:{selectedFacet}";
		}
	}

	private static string EncodeQueryPayload(string queryPayload)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(queryPayload);
		return Convert.ToBase64String(bytes);
	}

	[return: NotNullIfNotNull(nameof(encodedPayload))]
	private static string? DecodeQueryPayload(string? encodedPayload)
	{
		if (encodedPayload == null)
		{
			return null;
		}

		byte[] bytes = Convert.FromBase64String(encodedPayload);
		string jsonString = Encoding.UTF8.GetString(bytes);
		return jsonString;
	}
}