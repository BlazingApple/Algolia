using BlazingApple.Algolia.Models;
using Microsoft.AspNetCore.Components;

namespace BlazingApple.Algolia.Components.Results;

/// <summary>A single result in the list of Algolia search results.</summary>
public partial class SearchResultRow<T> : ComponentBase
{
	/// <summary>The result.</summary>
	[Parameter, EditorRequired]
	public required SearchResult<T> Result { get; set; }

	/// <summary>The child content.</summary>
	[Parameter]
	public RenderFragment<T>? ChildContent { get; set; }
}
