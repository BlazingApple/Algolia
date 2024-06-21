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
	private SearchResponse<SearchResult<T>>? _response;
	private int _page;
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

	[Inject]
	private IAlgoliaSearchService SearchService { get; set; } = null!;

	/// <inheritdoc />
	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		if (_response is null)
			await PerformSearch(true, false);
	}

	private async Task Search() => await PerformSearch(false, true);

	private async Task<Query> GetQuery()
	{
		Query query = new(SearchQuery)
		{
			HitsPerPage = _pageSize,
			Page = _page,
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
		}
		else
		{
			query = await GetQuery();
		}

		_response = await SearchService.Search<T>(query);
		StateHasChanged();
		if (updateUrl)
			UpdateUrl(query);
		_page = _response.Page;
		_isLoading = false;
	}

	private void UpdateUrl(Query query)
	{
		JsonSerializerOptions options = new(JsonSerializerDefaults.Web);
		options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;

		string queryPayload = JsonSerializer.Serialize(query, options);
		Url newUri = NavManager.Uri.SetQueryParam("query", EncodeQueryPayload(queryPayload));

		NavManager.NavigateTo(newUri, false, true);
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
			return null;

		byte[] bytes = Convert.FromBase64String(encodedPayload);
		string jsonString = Encoding.UTF8.GetString(bytes);
		return jsonString;
	}
}